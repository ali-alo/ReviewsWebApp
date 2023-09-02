using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Services.Interfaces;

namespace ReviewsWebApp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IImageService _imageService;
        public ReviewsController(IImageService imageService)
        {
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            ViewData["ContainerLink"] = _imageService.GetContainerLink();
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewDto reviewDto)
        {
            await _imageService.UploadImageToAzure(reviewDto.File);
            return RedirectToAction("Index");
        }
    }
}
