using ReviewsWebApp.Models;
using System.Text.RegularExpressions;

namespace ReviewsWebApp.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllTags();
        List<Tag> GetTagsFromString(string input);
    }
}
