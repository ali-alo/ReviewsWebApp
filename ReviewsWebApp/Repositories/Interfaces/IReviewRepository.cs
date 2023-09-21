using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<ReviewDetailsDto>> GetAllReviews();
        Task<ReviewDetailsDto?> GetReviewDetailsDto(int id);
        Task<ReviewDto?> GetReviewDtoById(int id);
        Task CreateReview(Review review);
        Task<bool> UpdateReview(Review review);
        Task<bool> DeleteReview(int id);
        Task<int> GetReviewsCount(int reviewItemId);
        Task<decimal> GetReviewsAverage(int reviewItemId);
        Task<int> UserAlreadyLeftReview(int reviewItemId, string userId);
        Task<List<ReviewDetailsDto>> GetMostPopularReviews(int takeAmount = 3, int skipAmount = 0);
        Task<List<ReviewDetailsDto>> GetMoreRecentReviews(int takeAmount = 3, int skipAmount = 0);
        Task<List<ReviewDetailsDto>> GetReviewItemReviews(int reviewItemId);
        Task<List<ReviewDetailsDto>> GetUserReviews(string userId);
        Task<List<ReviewDetailsDto>> GetUserLikedReviews(string userId);
    }
}
