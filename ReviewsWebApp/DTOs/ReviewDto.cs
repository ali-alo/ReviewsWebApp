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
        [Range(1, 10)]
        public decimal Grade { get; set; }
        public string TagsInput { get; set; } = string.Empty;
        public int ReviewItemId { get; set; }
        public string? CreatorId { get; set; } = string.Empty;
        public List<string> ImageGuids { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
    }
}
