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

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
