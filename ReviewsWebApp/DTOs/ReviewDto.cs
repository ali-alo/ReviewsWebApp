using ReviewsWebApp.Models;

namespace ReviewsWebApp.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MarkdownText { get; set; } = string.Empty;
        public List<Image> Images { get; set; } = new List<Image>();
        public decimal Grade { get; set; }
        public string? CreatorFirstName { get; set; }
        public string? CreatorLastName { get; set; }
        public string? CreatorId { get; set; }
        public string ReviewItemTitle { get; set; }
        public string ReviewItemImageGuid { get; set; }
        public int ReviewItemId { get; set; }
    }
}
