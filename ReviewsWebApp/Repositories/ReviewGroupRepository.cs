using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;

namespace ReviewsWebApp.Repositories
{
    public class ReviewGroupRepository : IReviewGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateGroup(ReviewGroup group)
        {
            if (NameIsDuplicated(group))
                return;
            await _context.ReviewsGroup.AddAsync(group);
            await _context.SaveChangesAsync();
        }

        private bool NameIsDuplicated(ReviewGroup group)
        {
            return _context.ReviewsGroup.Any(
                rg => rg.NameEn == group.NameEn.Trim().ToLower()) || _context.ReviewsGroup.Any(
                rg => rg.NameRu == group.NameRu.Trim());
        }

        public async Task<List<ReviewGroup>> GetAllGroups()
        {
            return await _context.ReviewsGroup.AsNoTracking().ToListAsync();
        }
    }
}
