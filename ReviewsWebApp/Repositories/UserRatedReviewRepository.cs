using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;

namespace ReviewsWebApp.Repositories
{
    public class UserRatedReviewRepository : IUserRatedReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRatedReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRatingToReview(UserRatedReview userRatedReview)
        {
            //var review = await _context.Reviews.Include(r => r.RatedReviews).FirstOrDefaultAsync(r => r.Id == userRatedReview.ReviewId);
            //if (review == null)
            //    return;
            //review.RatedReviews.Add(userRatedReview)
            _context.UserRatedReviews.Add(userRatedReview);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateReviewRatingIfExists(int reviewId, string userId, int rating)
        {
            var ratedReview = await _context.UserRatedReviews.FirstOrDefaultAsync(x => x.ReviewId == reviewId && x.UserId == userId);
            if (ratedReview == null)
                return false;
            ratedReview.Rating = rating;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
