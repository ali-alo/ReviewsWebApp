using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetReviewComments(int reviewId);
        Task CreateComment(Comment comment);
        Task<Comment?> GetComment(int commentId);
        Task<bool> DeleteComment(int commentId);
    }
}
