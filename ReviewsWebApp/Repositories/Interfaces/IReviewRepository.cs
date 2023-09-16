using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<ReviewDetailsDto>> GetAllReviews();
        Task<Review?> GetReviewById(int id);
        Task<ReviewDto?> GetReviewDtoById(int id);
        Task CreateReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(int id);
        Task<int> GetReviewsCount(int reviewItemId);
        Task<decimal> GetReviewsAverage(int reviewItemId);
        Task<List<Review>> FindReviews(string searchText);
        Task<int> UserAlreadyLeftReview(int reviewItemId, string userId);
    }
}
