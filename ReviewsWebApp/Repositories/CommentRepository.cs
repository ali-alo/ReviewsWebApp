using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;

namespace ReviewsWebApp.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateComment(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public Task DeleteComment(int reviewId, int commentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetReviewComments(int reviewId) =>
            await _context.Comments.Include(c => c.User).Where(c => c.ReviewId == reviewId).ToListAsync();

        public Task UpdateComment(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
