using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using ReviewsWebApp.Data;

namespace ReviewsWebApp.Middleware
{
    public class LockoutMiddleware
    {
        private readonly RequestDelegate _next;

        public LockoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> _userManager)
        {
            var user = await _userManager.GetUserAsync(context.User);
            if (user != null && user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                await context.SignOutAsync(IdentityConstants.ApplicationScheme);
                context.Response.Redirect("/Identity/Account/Lockout");
                return;
            }
            await _next(context);
        }

    }
}
