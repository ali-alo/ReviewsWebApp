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
        [MaxLength(100)]
        public string ReviewFor { get; set; } = string.Empty;
        public string MarkdownText { get; set; } = string.Empty;
        public List<Image> Images { get; set; } = new List<Image>();
        [Range(0, 10)]
        public decimal Grade { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public ReviewGroup ReviewGroup { get; set; }
        public int ReviewGroupId { get; set; }
        // null because if user deletes their account, the review will still be in the db
        public ApplicationUser? ApplicationUser { get; set; } 
        public string? CreatorId { get; set; }


    }
}
