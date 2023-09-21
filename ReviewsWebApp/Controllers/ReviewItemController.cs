﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.Areas.Identity;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;
using ReviewsWebApp.Services.Interfaces;
using ReviewsWebApp.ViewModels;

namespace ReviewsWebApp.Controllers
{
    public class ReviewItemController : Controller
    {
        private readonly IReviewItemRepository _repository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IReviewGroupRepository _reviewGroupRepository;
        private readonly ISearchService _searchService;

        public ReviewItemController(IReviewItemRepository repository, IImageService imageService,
            IMapper mapper, IReviewGroupRepository reviewGroupRepository, ISearchService searchService)
        {
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
            _reviewGroupRepository = reviewGroupRepository;
            _searchService = searchService;
        }

        public async Task<IActionResult> List()
        {
            string containerLink = _imageService.GetContainerLink();
            var items = await _repository.GetAllReviewItems();
            return View(new ReviewItemsListViewModel { ContainerLink = containerLink, Items = _mapper.Map<List<ReviewItemDto>>(items)});
        }
        
        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ReviewItemCreateViewModel { Groups = await GetGroups() };
            return View(viewModel);
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        [HttpPost]
        public async Task<ActionResult> Create(ReviewItemCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return await ResubmitForm(viewModel);
            string imageGuid = await UploadImageToAzure(viewModel.Review.ImageFile);
            if (string.IsNullOrEmpty(imageGuid))
                return await ResubmitForm(viewModel);
            var reviewItem = _mapper.Map<ReviewItem>(viewModel.Review);
            reviewItem.ImageGuid = imageGuid;
            await _repository.CreateReviewItem(reviewItem);
            return RedirectToAction("List");
        }

        private async Task<ActionResult> ResubmitForm(ReviewItemCreateViewModel viewModel)
        {
            var groups = await GetGroups();
            viewModel.Groups = groups;
            return View("Create", viewModel);
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var reviewItem = await _repository.GetReviewItemById(id);
            if (reviewItem == null)
                return RedirectToAction("List");
            var reviewDto = _mapper.Map<ReviewItemDto>(reviewItem);
            return View(reviewDto);
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        [HttpPut]
        public async Task<ActionResult> Edit(ReviewItemDto reviewItemDto)
        {
            var reviewItemCopy = await _repository.GetReviewItemById(reviewItemDto.Id);
            if (reviewItemCopy == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await TryUpdateItemImage(reviewItemDto, reviewItemCopy.ImageGuid))
            {
                ModelState.AddModelError("ImageError", "Couldn't upload an image");
                return View(ModelState);
            }
            
            var reviewItem = _mapper.Map<ReviewItem>(reviewItemDto);
            await _repository.UpdateReviewItem(reviewItem);
            return Ok();
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var reviewItem  = await _repository.GetReviewItemById(id);
            if (reviewItem is not null)
            {
                await _repository.DeleteReviewItem(id);
                await DeleteImageFromAzure(reviewItem.ImageGuid);
                foreach (var review in reviewItem.Reviews)
                    await _searchService.DeleteRecord(review.Id.ToString());
                return Ok();
            }
            return BadRequest();
        }

        public async Task<ActionResult> Details(int id)
        {
            var reviewItem = await _repository.GetReviewItemById(id);
            if (reviewItem is null)
                return NotFound();
            string containerLink = _imageService.GetContainerLink();
            var reviews = await _repository.GetReviewItemReviews(id);
            return View(new ReviewItemDetailsViewModel { ReviewItem = reviewItem, Reviews = reviews, ContainerLink = containerLink});
        }

        private async Task<bool> TryUpdateItemImage(ReviewItemDto reviewItemDto, string oldImageGuid)
        {
            await DeleteImageFromAzure(oldImageGuid);
            reviewItemDto.ImageGuid = await UploadImageToAzure(reviewItemDto.ImageFile);
            if (string.IsNullOrEmpty(reviewItemDto.ImageGuid))
                return false;
            return true;
        }

        private async Task<List<ReviewGroup>> GetGroups() => await _reviewGroupRepository.GetAllGroups();
        private async Task DeleteImageFromAzure(string imageGuid) => await _imageService.DeleteImageFromAzure(imageGuid);
        private async Task<string> UploadImageToAzure(IFormFile imageFile) => await _imageService.UploadImageToAzure(imageFile);

    }
}
