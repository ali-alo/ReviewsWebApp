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

        public ReviewsController(IImageService imageService,IReviewRepository reviewRepository, 
            ITagService tagService, IReviewItemRepository reviewItemRepository, IMapper mapper)
        {
            _imageService = imageService;
            _reviewRepository = reviewRepository;
            _tagService = tagService;
            _reviewItemRepository = reviewItemRepository;
            _mapper = mapper;
        }

        public async Task <IActionResult> Index()
        {
            var containerLink = _imageService.GetContainerLink();
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

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var reviewEditDto = await _reviewRepository.GetReviewDtoById(id);
            if (reviewEditDto == null)
                return NotFound();

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == reviewEditDto.CreatorId
                || User.IsInRole(ApplicationRoleTypes.Admin))
            {
                var model = await GetFormViewModel(reviewEditDto.ReviewItemId);
                if (model == null)
                    return BadRequest();
                model.Review = reviewEditDto;
                return View(model);
            }

            return Unauthorized();
        }

        public async Task<ActionResult> Details(int id)
        {
            var review = await _reviewRepository.GetReviewById(id);
            if (review == null)
                return NotFound();
            var containerLink = _imageService.GetContainerLink();
            return View(new ReviewDetailsViewModel { ContainerLink = containerLink, Review = review});
        }

        private async Task<List<Tag>> GetTags() => await _tagService.GetAllTags();

    }
}
