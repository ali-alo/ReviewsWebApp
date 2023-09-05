using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;

namespace ReviewsWebApp.Repositories
{
    public class ReviewItemRepository : IReviewItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateReviewItem(ReviewItem item)
        {
            await _context.ReviewsItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public Task DeleteReviewItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReviewItem>> GetAllReviewItems()
        {
            return await _context.ReviewsItems.AsNoTracking().ToListAsync();
        }

        public Task<ReviewItem?> GetReviewItemById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateReviewItem(ReviewItem item)
        {
            throw new NotImplementedException();
        }
    }
}
