using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.Areas.Identity;
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

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userRepository.GetUserDto(id);
            if (user == null)
                return NotFound();
            user.Reviews = await _reviewRepository.GetUserReviews(id);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id)
            {
                user.IsOwningAccount = true;
                user.LikedReviews = await _reviewRepository.GetUserLikedReviews(id);
            }
            return View(user);
        }

        // return URL is needed because users may like from review details or reviews index pages
        [Authorize]
        public async Task<IActionResult> ToggleReviewLike(int id, string returnUrl)
        {
            if (!await _userRepository.ToggleReviewLike(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                return RedirectToAction("Index", "Reviews");
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Reviews");
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public async Task<IActionResult> List(string searchQuery)
        {
            var users = string.IsNullOrEmpty(searchQuery) ? await _userRepository.GetAllUsers()
                : await _userRepository.SearchUsers(searchQuery);
            ViewData["SearchQuery"] = searchQuery;
            return View(users);
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public async Task<IActionResult> Block(string id)
        {
            await _userRepository.BlockUser(id);
            return RedirectToAction("Details", "User", new { id });
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public async Task<IActionResult> Unblock(string id)
        {
            await _userRepository.UnblockUser(id);
            return RedirectToAction("Details", "User", new { id });
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            await _userRepository.MakeAdmin(id);
            return RedirectToAction("Details", "User", new { id });
        }

        [Authorize(Roles = ApplicationRoleTypes.Admin)]
        public async Task<IActionResult> RemoveAdminRights(string id)
        {
            await _userRepository.RemoveAdminRights(id);
            return RedirectToAction("Details", "User", new { id });
        }
    }
}
