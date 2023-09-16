using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewFormViewModel
    {
        public ReviewDto Review { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        [ValidateNever]
        public string ContainerLink { get; set; }
        [ValidateNever]
        public ReviewItem ReviewItem { get; set; }
        public int ReviewsCount { get; set; }
        public decimal ReviewsAverage { get; set; }

    }
}
