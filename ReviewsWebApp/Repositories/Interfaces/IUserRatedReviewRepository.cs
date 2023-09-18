using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IUserRatedReviewRepository
    {
        Task AddRatingToReview(UserRatedReview userRatedReview);
        Task<bool> UpdateReviewRatingIfExists(int reviewId, string userId, int rating);
    }
}
