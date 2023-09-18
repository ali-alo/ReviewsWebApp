using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewDetailsViewModel
    {
        public ReviewDetailsDto ReviewDetails { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        [ValidateNever]
        public string ContainerLink { get; set; }
        [ValidateNever]
        public ReviewItem ReviewItem { get; set; }
        public int ReviewsCount { get; set; }
        public decimal ReviewsAverage { get; set; }
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public CommentFormDto CommentForm { get; set; } = new ();
    }
}
