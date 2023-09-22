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

        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await GetComment(commentId);
            if (comment == null)
                return false;    
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Comment?> GetComment(int commentId) => 
            await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

        public async Task<List<Comment>> GetReviewComments(int reviewId) =>
            await _context.Comments
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .Where(c => c.ReviewId == reviewId)
            .ToListAsync();

    }
}
