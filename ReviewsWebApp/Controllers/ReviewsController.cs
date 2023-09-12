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
        private readonly IReviewGroupRepository _reviewGroupRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ITagService _tagService;
        private readonly IReviewItemRepository _reviewItemRepository;
        private readonly IMapper _mapper;

        public ReviewsController(IImageService imageService, IReviewGroupRepository reviewGroupRepository,
            IReviewRepository reviewRepository, ITagService tagService, IReviewItemRepository reviewItemRepository,
            IMapper mapper)
        {
            _imageService = imageService;
            _reviewGroupRepository = reviewGroupRepository;
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
            var (tags, groups) = await GetTagsAndGroups();
            if (!await _reviewItemRepository.ReviewItemExists(id)) 
                return RedirectToAction("List", "ReviewItem");
            return View(new ReviewCreateViewModel { Tags = tags, Groups = groups, Review = new ReviewCreateDto { ReviewItemId = id } });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(ReviewCreateViewModel reviewCreateViewModel)
        {
            if (!await _reviewItemRepository.ReviewItemExists(reviewCreateViewModel.Review.ReviewItemId))
                return RedirectToAction("List", "ReviewItem");
            if (!ModelState.IsValid)
                return await ResubmitForm(reviewCreateViewModel);

            Review review = _mapper.Map<Review>(reviewCreateViewModel.Review);
            review.Images = await _imageService.UploadImagesToAzure(reviewCreateViewModel.Review.Files);
            review.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _reviewRepository.CreateReview(review);

            return RedirectToAction("Index");
        }

        private async Task<(List<Tag>, List<ReviewGroup>)> GetTagsAndGroups()
        {
            var tags = await _tagService.GetAllTags();
            var groups = await _reviewGroupRepository.GetAllGroups();
            return (tags, groups);
        }

        private async Task<ActionResult> ResubmitForm(ReviewCreateViewModel model)
        {
            var (tags, groups) = await GetTagsAndGroups();
            model.Tags = tags;
            model.Groups = groups;
            return View("Create", model);
        }

    }
}
