using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Extensions;
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

        public async Task<List<ReviewDetailsDto>> GetReviews(int pageNumber, int takeAmount = 8)
        {
            if (pageNumber < 0)
                return new List<ReviewDetailsDto>();
            if (pageNumber == 0)
                pageNumber++;
            return await _context.Reviews
                .IncludeCommon()
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => MapToDto(r))
                .Skip((pageNumber -1) * takeAmount)
                .Take(takeAmount)
                .ToListAsync();
        }

        public async Task<(int pagesCount, bool isFirstPage, bool isLastPage)> GetAllReviewsPaginationInfo(int pageNumber, int takeAmount = 8)
        {
            int reviewsCount = await _context.Reviews.CountAsync();
            return GetPagesInfo(reviewsCount, pageNumber, takeAmount);
        }

        public async Task<List<ReviewDetailsDto>> GetReviewsByTag(string tagName, int pageNumber, int takeAmount = 8)
        {
            if (pageNumber < 0)
                return new List<ReviewDetailsDto>();
            if (pageNumber == 0)
                pageNumber++;
            return await _context.Reviews
            .IncludeCommon()
            .Where(r => r.Tags.Any(t => t.Name == tagName.Trim().ToLower()))
            .OrderByDescending(r => r.UsersWhoLiked.Count())
            .Select(r => MapToDto(r))
            .Skip((pageNumber - 1) * takeAmount)
            .Take(takeAmount)
            .ToListAsync();
        }

        public async Task<(int pagesCount, bool isFirstPage, bool isLastPage)> GetReviewsByTagPaginationInfo(string tagName, int pageNumber, int takeAmount = 8)
        {
            int reviewsCount = await _context.Reviews
                .Where(r => r.Tags.Any(t => t.Name == tagName.Trim().ToLower()))
                .CountAsync();
            return GetPagesInfo(reviewsCount, pageNumber, takeAmount);
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

        public async Task<List<ReviewDetailsDto>> GetMostPopularReviews(int takeAmount = 3, int skipAmount = 0) => await 
            _context.Reviews
            .IncludeCommon()
            .OrderByDescending(r => r.UsersWhoLiked.Count)
            .Select(r => MapToDto(r))
            .Skip(skipAmount)
            .Take(takeAmount)
            .ToListAsync();

        public async Task<List<ReviewDetailsDto>> GetMoreRecentReviews(int takeAmount = 3, int skipAmount = 0) => await 
            _context.Reviews
            .IncludeCommon()
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => MapToDto(r))
            .Skip(skipAmount)
            .Take(takeAmount)
            .ToListAsync();

        public async Task<ReviewDetailsDto?> GetReviewDetailsDto(int id) => await
            _context.Reviews
            .IncludeCommon()
            .Where(r => r.Id == id)
            .Select(r => MapToDto(r))
            .FirstOrDefaultAsync();

        public async Task<List<ReviewDetailsDto>> GetReviewItemReviews(int reviewItemId) => await
            _context.Reviews
            .IncludeCommon()
            .Where(r => r.ReviewItem.Id == reviewItemId)
            .OrderByDescending(u => u.UsersWhoLiked.Count())
            .Select(r => MapToDto(r))
            .ToListAsync();

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
                        ImageGuids = r.Images.Select(img => img.ImageGuid).ToList(),
                        Tags = r.Tags,
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

        public async Task<bool> UpdateReview(Review review)
        {
            var reviewFromDb = _context.Reviews.Include(r => r.Images)
                                .Include(r => r.Tags)
                                .Single(r => r.Id == review.Id);
            if (reviewFromDb == null)
                return false;
            UpdateReviewProperties(reviewFromDb, review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ReviewDetailsDto>> GetUserReviews(string userId) => await
            _context.Reviews
            .IncludeCommon()
            .Where(r => r.CreatorId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => MapToDto(r))
            .ToListAsync();


        public async Task<List<ReviewDetailsDto>> GetUserLikedReviews(string userId) => await
            _context.Reviews
            .IncludeCommon()
            .Where(r => r.UsersWhoLiked.Any(u => u.Id == userId))
            .OrderByDescending(r => r.UsersWhoLiked.Count())
            .Select(r => MapToDto(r))
            .ToListAsync();

        public async Task<int> UserAlreadyLeftReview(int reviewItemId, string userId)
        {
            var existingReviewId = await _context.Reviews
                .Where(r => r.ReviewItemId == reviewItemId && r.CreatorId == userId)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
            return existingReviewId != 0 ? existingReviewId : 0;
        }

        private void UpdateReviewProperties(Review reviewFromDb, Review reviewUpdated)
        {
            reviewFromDb.Title = reviewUpdated.Title;
            reviewFromDb.MarkdownText = reviewUpdated.MarkdownText;
            reviewFromDb.Images = reviewUpdated.Images;
            reviewFromDb.Grade = reviewUpdated.Grade;
            reviewFromDb.Tags = reviewUpdated.Tags;
        }

        private static ReviewDetailsDto MapToDto(Review review) =>
            new ReviewDetailsDto
            {
                Id = review.Id,
                Title = review.Title,
                MarkdownText = review.MarkdownText,
                Images = review.Images,
                Grade = review.Grade,
                CreatedTime = review.CreatedAt,
                CreatorFirstName = review.ApplicationUser == null ? null : review.ApplicationUser.FirstName,
                CreatorLastName = review.ApplicationUser == null ? null : review.ApplicationUser.LastName,
                CreatorId = review.ApplicationUser == null ? null : review.ApplicationUser.Id,
                ReviewItemNameEn = review.ReviewItem.NameEn,
                ReviewItemNameRu = review.ReviewItem.NameRu,
                ReviewItemImageGuid = review.ReviewItem.ImageGuid,
                ReviewItemId = review.ReviewItem.Id,
                ReviewItemGroupNameEn = review.ReviewItem.ReviewGroup.NameEn,
                ReviewRatings = review.RatedReviews,
                UsersIdWhoLiked = review.UsersWhoLiked.Select(u => u.Id).ToList(),
                Tags = review.Tags,
            };

        private (int pagesCount, bool isFirstPage, bool isLastPage) GetPagesInfo(int reviewsCount, int pageNumber, int takeAmount = 8)
        {
            int pagesCount = (int)Math.Ceiling(reviewsCount / (decimal)takeAmount);
            bool isFirstPage = false;
            bool isLastPage = false;
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            if (pageNumber == 1)
                isFirstPage = true;
            if (pageNumber * takeAmount >= reviewsCount)
                isLastPage = true;
            return (pagesCount, isFirstPage, isLastPage);
        }
    }
}
