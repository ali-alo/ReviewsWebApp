﻿using Algolia.Search.Clients;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.Areas.Identity;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;
using ReviewsWebApp.Services.Interfaces;
using ReviewsWebApp.ViewModels;
using System.Security.Claims;

namespace ReviewsWebApp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IReviewRepository _reviewRepository;
        private readonly ITagService _tagService;
        private readonly IReviewItemRepository _reviewItemRepository;
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly ISearchService _searchService;

        public ReviewsController(IImageService imageService,IReviewRepository reviewRepository, 
            ITagService tagService, IReviewItemRepository reviewItemRepository, IMapper mapper,
            ICommentRepository commentRepository, ISearchService searchService)
        {
            _imageService = imageService;
            _reviewRepository = reviewRepository;
            _tagService = tagService;
            _reviewItemRepository = reviewItemRepository;
            _mapper = mapper;
            _commentRepository = commentRepository;
            _searchService = searchService;
        }

        public async Task<IActionResult> Index()
        {
            var containerLink =  _imageService.GetContainerLink();
            var reviews = await _reviewRepository.GetAllReviews();
            return View(new ReviewsIndexViewModel { Reviews = reviews, ContainerLink = containerLink });
        }

        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var reviewItem = await _reviewItemRepository.GetReviewItemById(id);
            if (reviewItem == null)
                return RedirectToAction("List", "ReviewItem");
            int existingReviewId = await _reviewRepository.UserAlreadyLeftReview(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (existingReviewId != 0) // user has left a review for this item already
                return RedirectToAction("Details", "Reviews", routeValues: new { id = existingReviewId });
            ReviewFormViewModel viewModel = await GetFormViewModel(reviewItem);
            return View(viewModel);
        }

        private async Task<ReviewFormViewModel> GetFormViewModel(ReviewItem reviewItem)
        {
            var tags = await GetTags();
            var containerLink = _imageService.GetContainerLink();
            var reviewsCount = await _reviewRepository.GetReviewsCount(reviewItem.Id);
            var reviewsAverage = await _reviewRepository.GetReviewsAverage(reviewItem.Id);
            var reviewCreateDto = new ReviewDto { ReviewItemId = reviewItem.Id };
            return new ReviewFormViewModel
            {
                Tags = tags,
                Review = reviewCreateDto,
                ContainerLink = containerLink,
                ReviewItem = reviewItem,
                ReviewsCount = reviewsCount,
                ReviewsAverage = reviewsAverage
            };
        }

        private async Task<ReviewFormViewModel?> GetFormViewModel(int reviewItemId)
        {
            var reviewItem = await _reviewItemRepository.GetReviewItemById(reviewItemId);
            if (reviewItem == null)
                return null;
            return await GetFormViewModel(reviewItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(ReviewFormViewModel reviewCreateViewModel)
        {
            if (!await _reviewItemRepository.ReviewItemExists(reviewCreateViewModel.Review.ReviewItemId))
                return RedirectToAction("List", "ReviewItem");
            if (!ModelState.IsValid)
            {
                var viewModel = await GetFormViewModel(reviewCreateViewModel.Review.ReviewItemId);
                if (viewModel == null)
                    return RedirectToAction("List", "ReviewItem");
                viewModel.Review = reviewCreateViewModel.Review;
                return View(viewModel);
            }

            Review review = _mapper.Map<Review>(reviewCreateViewModel.Review);
            review.Images = await _imageService.UploadImagesToAzure(reviewCreateViewModel.Review.Files);
            review.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            review.CreatedAt = DateTime.UtcNow;
            await _reviewRepository.CreateReview(review);

            await _searchService.AddRecord(new SearchDto(review.Id, review.Title, review.MarkdownText));

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var reviewDto = await _reviewRepository.GetReviewDtoById(id);
            if (reviewDto == null)
                return NotFound();

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == reviewDto.CreatorId
                || User.IsInRole(ApplicationRoleTypes.Admin))
            {
                var model = await GetFormViewModel(reviewDto.ReviewItemId);
                if (model == null)
                    return BadRequest();
                model.Review = reviewDto;
                return View(model);
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpPut] 
        public async Task<IActionResult> Edit(ReviewFormViewModel model)
        {
            var reviewCopy = await _reviewRepository.GetReviewDtoById(model.Review.Id);
            if (reviewCopy == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            
            // couldn't upload all images
            if (!await TryUpdateReviewImages(model.Review, reviewCopy.Images)) 
            {
                //delete images that were uploaded
                await DeleteReviewImagesFromAzure(model.Review.Images);
                ModelState.AddModelError("ImageUploadError", "Couldn't upload image(s)");
                return BadRequest(ModelState);
            }

            Review review = _mapper.Map<Review>(model.Review);
            review.Images = await _imageService.UploadImagesToAzure(model.Review.Files);
            if (!await _reviewRepository.UpdateReview(review))
            {
                ModelState.AddModelError("Update Error", "Couldn't update the record in the db");
                return BadRequest(ModelState);
            }
            await _searchService.UpdateRecord(new SearchDto(review.Id, review.Title, review.MarkdownText));
            return Ok();
            
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var review = await _reviewRepository.GetReviewDtoById(id);
            if (review == null)
                return BadRequest();
            if (!await _reviewRepository.DeleteReview(id))
                return BadRequest();
            await DeleteReviewImagesFromAzure(review.Images);
            await _searchService.DeleteRecord(id.ToString());
            return Ok();
        }

        public async Task<ActionResult> Details(int id)
        {
            var reviewDto = await _reviewRepository.GetReviewDetailsDto(id);
            if (reviewDto == null)
                return NotFound();
            var formModel = await GetFormViewModel(reviewDto.ReviewItemId);
            if (formModel == null)
                return BadRequest();
            var model = _mapper.Map<ReviewDetailsViewModel>(formModel);
            model.ReviewDetails = reviewDto;
            model.Comments = await _commentRepository.GetReviewComments(id);
            model.CommentForm.ReviewId = id;
            var userRatedReview = reviewDto.ReviewRatings.FirstOrDefault(r => r.ReviewId == id
                && r.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.CommentForm.Grade = userRatedReview == null ? 0 : userRatedReview.Rating;
            return View(model);
        }

        private async Task<List<Tag>> GetTags() => await _tagService.GetAllTags();

        private async Task<bool> TryUpdateReviewImages(ReviewDto reviewDto, IEnumerable<string> oldImageGuids)
        {
            await DeleteReviewImagesFromAzure(oldImageGuids);
            reviewDto.Images = await UploadReviewImagesToAzure(reviewDto.Files);

            if (reviewDto.Images.Any(ig => string.IsNullOrEmpty(ig)))
                return false;
            return true;
        }

        private async Task DeleteReviewImagesFromAzure(IEnumerable<string> oldImageGuids)
        {
            foreach (var imgGuid in oldImageGuids)
                await _imageService.DeleteImageFromAzure(imgGuid);
        }
        private async Task<List<string>> UploadReviewImagesToAzure(IEnumerable<IFormFile> imageFiles)
        {
            var imageGuidList = new List<string>();
            foreach (var imageFile in imageFiles)
                imageGuidList.Add(await _imageService.UploadImageToAzure(imageFile));
            return imageGuidList;

        }
    }
}
