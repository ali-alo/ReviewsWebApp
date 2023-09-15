using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewsIndexViewModel
    {
        public IEnumerable<ReviewDto> Reviews { get; set; }
        public string ContainerLink { get; set; }
    }
}
