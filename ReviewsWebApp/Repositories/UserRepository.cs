using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;

namespace ReviewsWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task BlockUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DislikeReview(string userId, int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            var user = await GetUserWithLikesById(userId);
            if (review == null || user == null || !user.LikedReviews.Contains(review))
                return false;
            user.LikedReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> LikeReview(string userId, int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            var user = await GetUserWithLikesById(userId);
            if (review == null || user == null || user.LikedReviews.Contains(review))
                return false;
            user.LikedReviews.Add(review);
            await _context.SaveChangesAsync();
            return true;

        }

        public Task MakeAdmin(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser?> GetUserWithLikesById(string userId) =>
            await _context.Users.Include(u => u.LikedReviews).FirstOrDefaultAsync(u => u.Id == userId);
    }
}
