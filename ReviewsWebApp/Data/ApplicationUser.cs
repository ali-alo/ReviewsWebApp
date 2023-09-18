using Microsoft.AspNetCore.Identity;
using ReviewsWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public List<Review> Reviews { get; set; } = new List<Review>();
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public List<Review> LikedReviews { get; set; }
        public List<Comment> Comments { get; set; }
        public List<UserRatedReview> RatedReviews { get; set; }
    }
}
