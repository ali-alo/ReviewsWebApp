using ReviewsWebApp.DTOs;

namespace ReviewsWebApp.Services.Interfaces
{
    public interface ISearchService
    {
        Task AddRecord(SearchDto record);
        Task DeleteRecord(string objectId);
        Task<SearchDto?> GetRecord(string objectId);
        Task UpdateRecord(SearchDto record);
    }
}
