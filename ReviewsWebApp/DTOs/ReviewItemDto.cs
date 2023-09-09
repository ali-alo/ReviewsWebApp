using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReviewsWebApp.DTOs
{
    public class ReviewItemDto : ReviewItemCreateDto
    {
        public int Id { get; set; }
        [BindNever]
        public string ImageGuid { get; set; } = "";
    }
}
