using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IReviewGroupRepository
    {
        Task<List<ReviewGroup>> GetAllGroups();
        Task CreateGroup(ReviewGroup group);
        
    }
}
