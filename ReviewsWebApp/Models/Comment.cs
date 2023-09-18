using ReviewsWebApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [MaxLength(500)]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Review Review { get; set; }
        public int ReviewId { get; set; }
    }
}
