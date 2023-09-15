using System.ComponentModel.DataAnnotations;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.DTOs
{
    public class ReviewCreateDto
    {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ReviewResource))]
        [MaxLength(40, ErrorMessageResourceType = typeof(Resources.Models.ReviewResource), ErrorMessageResourceName = "TitlePropertyLong")]
        public string Title { get; set; } = string.Empty;
        public string MarkdownText { get; set; } = string.Empty;
        [Range(0, 10)]
        public decimal Grade { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public int ReviewItemId { get; set; }
    }
}
