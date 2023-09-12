using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllReviews();
        Task<Review?> GetReviewById(int id);
        Task CreateReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(int id);
        Task<List<Review>> FindReviews(string searchText);
    }
}
