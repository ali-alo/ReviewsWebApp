using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.DTOs;
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
            return await _context.ReviewsItems.Include(r => r.ReviewGroup).AsNoTracking().ToListAsync();
        }

        public async Task<ReviewItem?> GetReviewItemById(int id)
        {
            return await _context.ReviewsItems.Include(r => r.ReviewGroup).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ReviewDetailsDto>> GetReviewItemReviews(int id)
        {
            return await _context.Reviews
                .Where(r => r.ReviewItem.Id == id)
                .Select(r => new ReviewDetailsDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    MarkdownText = r.MarkdownText,
                    Images = r.Images,
                    Grade = r.Grade,
                    CreatedTime = r.CreatedAt,
                    CreatorFirstName = r.ApplicationUser == null ? null : r.ApplicationUser.FirstName,
                    CreatorLastName = r.ApplicationUser == null ? null : r.ApplicationUser.LastName,
                    CreatorId = r.ApplicationUser == null ? null : r.ApplicationUser.Id,
                    ReviewItemNameEn = r.ReviewItem.NameEn,
                    ReviewItemNameRu = r.ReviewItem.NameRu,
                    ReviewItemImageGuid = r.ReviewItem.ImageGuid,
                    ReviewItemId = r.ReviewItem.Id,
                    ReviewItemGroupNameEn = r.ReviewItem.ReviewGroup.NameEn,
                    ReviewRatings = r.RatedReviews,
                    UsersIdWhoLiked = r.UsersWhoLiked.Select(u => u.Id).ToList()
                })
                .OrderByDescending(u => u.UsersIdWhoLiked.Count())
                .ToListAsync();
        }

        public async Task<bool> ReviewItemExists(int id)
        {
            return await _context.ReviewsItems.AnyAsync(i => i.Id == id);
        }

        public async Task UpdateReviewItem(ReviewItem item)
        {
            var reviewItem = await GetReviewItemById(item.Id);
            if (reviewItem == null)
                return;
            reviewItem.NameEn = item.NameEn;
            reviewItem.NameRu = item.NameRu;
            reviewItem.DescriptionEn = item.DescriptionEn;
            reviewItem.DescriptionRu = item.DescriptionRu;
            reviewItem.ImageGuid = item.ImageGuid;
            await _context.SaveChangesAsync();
        }
    }
}
