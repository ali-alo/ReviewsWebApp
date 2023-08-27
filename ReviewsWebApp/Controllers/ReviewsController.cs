using Microsoft.AspNetCore.Mvc;

namespace ReviewsWebApp.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
