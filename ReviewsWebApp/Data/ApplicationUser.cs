using Microsoft.AspNetCore.Identity;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
