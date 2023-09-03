using ReviewsWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.DTOs
{
    public class ReviewCreateDto
    {
        public List<IFormFile> Files { get; set; }
        [MaxLength(40, ErrorMessageResourceName = "TitlePropertyLong")]
        public string Title { get; set; } = string.Empty;
        public string MarkdownText { get; set; } = string.Empty;
        [Range(0, 10)]
        public decimal Grade { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public int ReviewGroupId { get; set; }

    }
}
