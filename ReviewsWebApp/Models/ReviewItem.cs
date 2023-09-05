using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.Models
{
    public class ReviewItem
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string ImageGuid { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
