using ReviewsWebApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        // TODO: Add error message reading from .resx files
        [MaxLength(40, ErrorMessageResourceName = "TitlePropertyLong")]
        public string Title { get; set; } = string.Empty;
        public string MarkdownText { get; set; } = string.Empty;
        public List<Image> Images { get; set; } = new List<Image>();
        [Range(0, 10)]
        public decimal Grade { get; set; }
        // null because if user deletes their account, the review will still be in the db
        public ApplicationUser? ApplicationUser { get; set; }
        public string? CreatorId { get; set; }
        public ReviewItem ReviewItem { get; set; }
        public int ReviewItemId { get; set;}
        public DateTime CreatedAt { get; set; }

    }
}
