using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.DTOs;
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

        public async Task<List<ReviewDetailsDto>> GetAllReviews()
        {
            return await _context.Reviews
                .Include(r => r.ApplicationUser)
                .Include(r => r.ReviewItem)
                .Include(r => r.Images)
                .Select(r =>
                    new ReviewDetailsDto
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
                        ReviewItemGroupNameEn = r.ReviewItem.ReviewGroup.NameEn
                    }).ToListAsync();
        }

        public async Task<Review?> GetReviewById(int id) =>
            await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

        public async Task<ReviewDto?> GetReviewDtoById(int id)
        {
            return await _context.Reviews
                .Select(r => 
                    new ReviewDto
                    {
                        Id= r.Id,
                        Title = r.Title,
                        MarkdownText = r.MarkdownText,
                        Grade= r.Grade,
                        ReviewItemId = r.ReviewItemId,
                        CreatorId = r.CreatorId
                        // TODO: add comments and likes amount
                    })
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<decimal> GetReviewsAverage(int reviewItemId) =>
            await _context.Reviews
            .Where(r => r.ReviewItemId == reviewItemId)
            .Select(r => r.Grade)
            .DefaultIfEmpty()
            .AverageAsync();

        public async Task<int> GetReviewsCount(int reviewItemId) => 
            await _context.Reviews
            .Where(r => r.ReviewItemId == reviewItemId)
            .CountAsync();

        public Task UpdateReview(Review review)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UserAlreadyLeftReview(int reviewItemId, string userId)
        {
            var existingReviewId = await _context.Reviews
                .Where(r => r.ReviewItemId == reviewItemId && r.CreatorId == userId)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
            return existingReviewId != 0 ? existingReviewId : 0;
        }
    }
}
