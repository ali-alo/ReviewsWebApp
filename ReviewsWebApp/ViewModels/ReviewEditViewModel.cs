using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewEditViewModel
    {
        public ReviewEditDto Review { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
