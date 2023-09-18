using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewItemSummaryViewModel
    {
        public string ContainerLink { get; set; }
        public ReviewItem ReviewItem { get; set; }
        public decimal ReviewsAverage { get; set; }
        public int ReviewsCount { get; set; }
        public ReviewItemSummaryViewModel(string containerLink,  decimal reviewsAverage, 
            int reviewsCount, ReviewItem reviewItem)
        {
            ContainerLink = containerLink;
            ReviewsAverage = reviewsAverage;
            ReviewsCount = reviewsCount;
            ReviewItem = reviewItem;
        }
    }
}
