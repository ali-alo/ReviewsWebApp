using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IReviewItemRepository
    {
        Task<List<ReviewItem>> GetAllReviewItems();
        Task<ReviewItem?> GetReviewItemById(int id);
        Task CreateReviewItem(ReviewItem item);
        Task UpdateReviewItem(ReviewItem item);
        Task DeleteReviewItem(int id);
        Task<bool> ReviewItemExists(int id);
        Task<ReviewItem?> GetReviewItemWithReviews(int id);
    }
}
