using ReviewsWebApp.Data;
using ReviewsWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.DTOs
{
    public class CommentFormDto
    {
        public int Id { get; set; }
        [MaxLength(500)]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
        public int ReviewId { get; set; }
        [Range(0, 5)]
        public int Grade { get; set; }
    }
}
