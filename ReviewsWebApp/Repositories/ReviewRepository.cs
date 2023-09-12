using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;

namespace ReviewsWebApp.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public Task DeleteReview(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Review>> FindReviews(string searchText)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Review>> GetAllReviews()
        {
            return await _context.Reviews.Include(r => r.ApplicationUser).Include(r => r.ReviewGroup)
                .Include(r => r.ReviewItem).Include(r => r.Images).AsNoTracking().ToListAsync();
        }

        public Task<Review?> GetReviewById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateReview(Review review)
        {
            throw new NotImplementedException();
        }
    }
}
