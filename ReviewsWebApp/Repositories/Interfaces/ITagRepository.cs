using ReviewsWebApp.Models;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllTags();
        Task<List<Tag>> GetTagsFromInput(string input);
        Task DeleteTagsWithNoReviews();
    }
}
