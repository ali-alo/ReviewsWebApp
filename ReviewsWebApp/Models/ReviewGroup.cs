using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.Models
{
    public class ReviewGroup
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = "";
        public Locale Locale { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
