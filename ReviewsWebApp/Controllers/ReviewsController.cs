using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.Areas.Identity;
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

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public IActionResult Index()
        {
            ViewData["ContainerLink"] = _imageService.GetContainerLink();
            return View();
        }

        [Authorize]
        public IActionResult Create()
        {
            return View(new ReviewCreateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(ReviewCreateDto reviewDto)
        {
                foreach (var imgFile in reviewDto.Files)
                {
                    await _imageService.UploadImageToAzure(imgFile);
                }
            return RedirectToAction("Index");
        }
    }
}
