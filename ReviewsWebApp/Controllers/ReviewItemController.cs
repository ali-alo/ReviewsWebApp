using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.Areas.Identity;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;
using ReviewsWebApp.Services.Interfaces;

namespace ReviewsWebApp.Controllers
{
    [Authorize(Roles = ApplicationRoleTypes.Admin)]
    public class ReviewItemController : Controller
    {
        private readonly IReviewItemRepository _repository;
        private readonly IImageService _imageService;

        public ReviewItemController(IReviewItemRepository repository, IImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
        }

        public async Task<IActionResult> List()
        {
            ViewData["ContainerLink"] = _imageService.GetContainerLink();
            var items = await _repository.GetAllReviewItems();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ReviewItemCreateDto reviewItemCreateDto)
        {
            if (!ModelState.IsValid)
                return View(reviewItemCreateDto);
            string imageGuid = await _imageService.UploadImageToAzure(reviewItemCreateDto.ImageFile);
            if (string.IsNullOrEmpty(imageGuid)) 
                return View(reviewItemCreateDto);
            var reviewItem = new ReviewItem() { Name = reviewItemCreateDto.Name, ImageGuid = imageGuid };
            await _repository.CreateReviewItem(reviewItem);
            return RedirectToAction("List");
        }
    }
}
