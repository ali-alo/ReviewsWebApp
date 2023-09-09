using AutoMapper;
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

        public ReviewItemController(IReviewItemRepository repository, IImageService imageService,
            IMapper mapper)
        {
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<IActionResult> List()
        {
            string containerLink = _imageService.GetContainerLink();
            var items = await _repository.GetAllReviewItems();
            return View(new ReviewItemsListViewModel { ContainerLink = containerLink, Items = _mapper.Map<List<ReviewItemDto>>(items)});
        }
        
        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        [HttpPost]
        public async Task<ActionResult> Create(ReviewItemCreateDto reviewItemCreateDto)
        {
            if (!ModelState.IsValid)
                return View(reviewItemCreateDto);
            string imageGuid = await _imageService.UploadImageToAzure(reviewItemCreateDto.ImageFile);
            if (string.IsNullOrEmpty(imageGuid)) 
                return View(reviewItemCreateDto);
            var reviewItem = new ReviewItem() { Name = reviewItemCreateDto.Name, ImageGuid = imageGuid, Description = reviewItemCreateDto.Description };
            await _repository.CreateReviewItem(reviewItem);
            return RedirectToAction("List");
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var reviewItem = await _repository.GetReviewItemById(id);
            if (reviewItem == null)
                return RedirectToAction("List");
            return View(_mapper.Map<ReviewItemDto>(reviewItem));
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        [HttpPost]
        public async Task<ActionResult> Edit(ReviewItemDto reviewItemDto)
        {
            if (!ModelState.IsValid)
                return View(reviewItemDto);
            if (string.IsNullOrEmpty(reviewItemDto.ImageGuid))
            {
                reviewItemDto.ImageGuid = await _imageService.UploadImageToAzure(reviewItemDto.ImageFile);
                if (string.IsNullOrEmpty(reviewItemDto.ImageGuid))
                    return View(reviewItemDto);
            }
            await _repository.UpdateReviewItem(_mapper.Map<ReviewItem>(reviewItemDto));
            return RedirectToAction("List");
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteReviewItem(id);
            return RedirectToAction("List");
        }
    }
}
