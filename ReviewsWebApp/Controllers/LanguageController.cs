using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ReviewsWebApp.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult SwitchLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
