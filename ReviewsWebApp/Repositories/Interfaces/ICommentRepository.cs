using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetReviewComments(int reviewId);
        Task CreateComment(Comment comment);
        Task UpdateComment(Comment comment);
        Task DeleteComment(int reviewId, int commentId);
    }
}
