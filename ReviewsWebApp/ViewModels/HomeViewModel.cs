using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ReviewDetailsDto> LatestReviews { get; set; }
        public IEnumerable<ReviewDetailsDto> MostLikedReviews { get; set; }
        public IList<Tag> Tags { get; set; }

        public HomeViewModel(IEnumerable<ReviewDetailsDto> latestReviews, IEnumerable<ReviewDetailsDto> mostLikedReviews, IList<Tag> tags)
        {
            LatestReviews = latestReviews;
            MostLikedReviews = mostLikedReviews;
            Tags = tags;
        }
    }
}
