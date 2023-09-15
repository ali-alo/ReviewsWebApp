using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.DTOs
{
    public class ReviewItemDto : ReviewItemCreateDto
    {
        public int Id { get; set; }
        [BindNever]
        public string ImageGuid { get; set; } = "";
        [ValidateNever]
        public ReviewGroup ReviewGroup { get; set; }
    }
}
