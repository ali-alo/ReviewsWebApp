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

        public async Task DeleteReviewItem(int id)
        {
            var reviewItem = await GetReviewItemById(id);
            if (reviewItem == null)
                return;
            _context.ReviewsItems.Remove(reviewItem);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReviewItem>> GetAllReviewItems()
        {
            return await _context.ReviewsItems.AsNoTracking().ToListAsync();
        }

        public async Task<ReviewItem?> GetReviewItemById(int id)
        {
            return await _context.ReviewsItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ReviewItemExists(int id)
        {
            return await _context.ReviewsItems.AnyAsync(i => i.Id == id);
        }

        public async Task UpdateReviewItem(ReviewItem item)
        {
            var review = await GetReviewItemById(item.Id);
            if (review == null)
                return;
            review.Name = item.Name;
            review.Description = item.Description;
            review.ImageGuid = item.ImageGuid;
            await _context.SaveChangesAsync();
        }
    }
}
