using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.Extensions
{
    public static class ReviewExtensions
    {
        public static IQueryable<Review> IncludeCommon(this IQueryable<Review> query)
        {
            return query
                .Include(r => r.ApplicationUser)
                .Include(r => r.ReviewItem)
                .   ThenInclude(ri => ri.ReviewGroup)
                .Include(r => r.Images)
                .Include(r => r.RatedReviews)
                .Include(r => r.UsersWhoLiked)
                .Include(r => r.Tags);
        }
    }
}
