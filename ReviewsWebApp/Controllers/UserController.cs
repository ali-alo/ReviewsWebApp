using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.Repositories.Interfaces;
using System.Security.Claims;

namespace ReviewsWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IReviewRepository _reviewRepository;

        public UserController(IUserRepository userRepository, IReviewRepository reviewRepository)
        {
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // redirect URL is needed because users may like from review details or reviews index pages
        [Authorize]
        public async Task<IActionResult> DislikeReview(int id, string returnUrl)
        { 
            if (!await _userRepository.DislikeReview(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                    return RedirectToAction("Index", "Reviews");
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Reviews");
        }

        [Authorize]
        public async Task<IActionResult> LikeReview(int id, string returnUrl)
        {
            if (! await _userRepository.LikeReview(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                    return RedirectToAction("Index", "Reviews");
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Reviews");
        }
    }
}
