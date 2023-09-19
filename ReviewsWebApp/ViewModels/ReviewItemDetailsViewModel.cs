using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewItemDetailsViewModel
    {
        public ReviewItem ReviewItem { get; set; }
        public IEnumerable<ReviewDetailsDto> Reviews { get; set; }
        public string ContainerLink { get; set; }
    }
}
