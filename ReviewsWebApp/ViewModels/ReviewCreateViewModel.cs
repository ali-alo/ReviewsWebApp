using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewCreateViewModel
    {
        public ReviewCreateDto Review { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<ReviewGroup> Groups { get; set; } = new List<ReviewGroup>();

    }
}
