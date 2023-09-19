using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Repositories.Interfaces;
using System.Security.Claims;

namespace ReviewsWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult List()
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

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userRepository.GetUserDto(id);
            if (user == null)
                return NotFound();
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id)
                user.IsOwningAccount = true;
            return View(user);
        }
    }
}
