using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ReviewDetailsDto> LatestReviews { get; set; }
        public IEnumerable<ReviewDetailsDto> MostLikedReviews { get; set; }
        public IEnumerable<Tag> Tags { get; set; }

        public HomeViewModel(IEnumerable<ReviewDetailsDto> latestReviews, IEnumerable<ReviewDetailsDto> mostLikedReviews, IEnumerable<Tag> tags)
        {
            LatestReviews = latestReviews;
            MostLikedReviews = mostLikedReviews;
            Tags = tags;
        }
    }
}
