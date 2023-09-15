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

        public async Task<List<ReviewDto>> GetAllReviews()
        {
            return await _context.Reviews
                .Include(r => r.ApplicationUser)
                .Include(r => r.ReviewItem)
                .Include(r => r.Images)
                .Select(r =>
                    new ReviewDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        MarkdownText = r.MarkdownText,
                        Images = r.Images,
                        Grade = r.Grade,
                        CreatorFirstName = r.ApplicationUser == null ? null : r.ApplicationUser.FirstName,
                        CreatorLastName = r.ApplicationUser == null ? null : r.ApplicationUser.LastName,
                        CreatorId = r.ApplicationUser == null ? null : r.ApplicationUser.Id,
                        ReviewItemTitle = r.Title,
                        ReviewItemImageGuid = r.ReviewItem.ImageGuid,
                        ReviewItemId = r.ReviewItem.Id
                    }).ToListAsync();
        }

        public async Task<ReviewEditDto?> GetReviewEditDtoById(int id)
        {
            return await _context.Reviews
                .Include(r => r.ReviewItem)
                .Select(r => 
                    new ReviewEditDto
                    {
                        Id= r.Id,
                        Title = r.Title,
                        MarkdownText = r.MarkdownText,
                        Grade= r.Grade,
                        ReviewItemTitleEn = r.ReviewItem.NameEn,
                        ReviewItemTitleRu = r.ReviewItem.NameRu,
                        CreatorId = r.CreatorId
                        // TODO: add comments and likes amount
                    })
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task UpdateReview(Review review)
        {
            throw new NotImplementedException();
        }
    }
}
