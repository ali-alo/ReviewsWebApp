using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.Models
{
    public class ReviewGroup
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string NameEn { get; set; }
        [MaxLength(50)]
        [Required]
        public string NameRu { get; set; }
        public List<ReviewItem> ReviewItems { get; set; } = new List<ReviewItem>();
    }
}
