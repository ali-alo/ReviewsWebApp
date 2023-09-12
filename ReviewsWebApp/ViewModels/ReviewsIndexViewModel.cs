using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewsIndexViewModel
    {
        public IEnumerable<Review> Reviews { get; set; }
        public string ContainerLink { get; set; }
    }
}
