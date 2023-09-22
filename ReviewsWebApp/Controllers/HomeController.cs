using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;
using ReviewsWebApp.ViewModels;
using System.Diagnostics;

namespace ReviewsWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ITagRepository _tagRepository;

        public HomeController(IReviewRepository reviewRepository, 
            ITagRepository tagRepository)
        {
            _reviewRepository = reviewRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index()
        {
            var latestReviews = await _reviewRepository.GetMostRecentReviews();
            var mostPopularReview = await _reviewRepository.GetMostPopularReviews();
            var tags = await _tagRepository.GetAllTags();
            var model = new HomeViewModel(latestReviews, mostPopularReview, tags);
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}