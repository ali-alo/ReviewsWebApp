using ReviewsWebApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.Models
{
    public class UserRatedReview
    {
        public int Id { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Review Review { get; set; }
        public int ReviewId { get; set; }
    }
}
