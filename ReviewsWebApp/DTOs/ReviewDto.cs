using System.ComponentModel.DataAnnotations;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ReviewResource))]
        [MaxLength(40, ErrorMessageResourceType = typeof(Resources.Models.ReviewResource), ErrorMessageResourceName = "TitlePropertyLong")]
        public string Title { get; set; } = string.Empty;
        public string MarkdownText { get; set; } = string.Empty;
        [Range(1, 10, ErrorMessage = "HEEEY")]
        public decimal Grade { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public int ReviewItemId { get; set; }
        public string? CreatorId { get; set; } = "";
        public List<string> Images { get; set; } = new();
    }
}
