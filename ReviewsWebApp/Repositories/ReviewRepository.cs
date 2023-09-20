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

        public async Task<bool> DeleteReview(int id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(review => review.Id == id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
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
                .Include(r => r.RatedReviews)
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
                }).ToListAsync();
        }

        public Task<ReviewDetailsDto?> GetReviewDetailsDto(int id) =>
            _context.Reviews.Select(r => new ReviewDetailsDto
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
            }).FirstOrDefaultAsync(r => r.Id == id);

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
                        CreatorId = r.CreatorId,
                        Images = r.Images.Select(img => img.ImageGuid).ToList(),
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

        public async Task<bool> ReviewExists(int reviewItemId) =>
            await _context.Reviews.AnyAsync(r => r.Id == reviewItemId);

        public async Task<bool> UpdateReview(Review review)
        {
            var reviewFromDb = _context.Reviews.Include(r => r.Images).Single(r => r.Id == review.Id);
            if (reviewFromDb == null)
                return false;
            reviewFromDb.Title = review.Title;
            reviewFromDb.MarkdownText = review.MarkdownText;
            reviewFromDb.Images = review.Images;
            reviewFromDb.Grade = review.Grade;
            await _context.SaveChangesAsync();
            return true;
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
