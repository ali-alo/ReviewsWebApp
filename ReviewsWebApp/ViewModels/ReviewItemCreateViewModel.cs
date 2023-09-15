using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewItemCreateViewModel
    {
        [BindNever]
        public IEnumerable<ReviewGroup> Groups { get; set; } = new List<ReviewGroup>();
        public ReviewItemCreateDto Review { get; set; }
    }
}
