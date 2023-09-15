using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<ReviewDto>> GetAllReviews();
        Task<ReviewEditDto?> GetReviewEditDtoById(int id);
        Task CreateReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(int id);
        Task<List<Review>> FindReviews(string searchText);
    }
}
