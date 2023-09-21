using Algolia.Search.Clients;
using Microsoft.Extensions.Options;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Options;
using ReviewsWebApp.Services.Interfaces;

namespace ReviewsWebApp.Services
{
    public class SearchService : ISearchService
    {

        private readonly SearchClient _client;
        private readonly SearchIndex _index;
        private readonly ILogger<SearchService> _logger;

        public SearchService(IOptions<AlgoliaOptions> algoliaOptions, ILogger<SearchService> logger)
        {
            _client = new SearchClient(algoliaOptions.Value.ApplicationId, algoliaOptions.Value.APIKey);
            _index = _client.InitIndex(algoliaOptions.Value.IndexName);
            _logger = logger;
        }

        public async Task AddRecord(SearchDto record)
        {
            try
            {
                await _index.SaveObjectAsync(record);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Couldn't add the record to the Algolia", ex.Message);
            }
        }

        public async Task DeleteRecord(string objectId)
        {
            var record = await GetRecord(objectId);
            if (record == null)
            {
                _logger.LogError("Couldn't retrieve the record from Angolia");
                return;
            }
            await _index.DeleteObjectAsync(objectId);
            _logger.LogInformation("Successfully deleted a record");
        }

        public async Task<SearchDto?> GetRecord(string objectId)
        {
            return await _index.GetObjectAsync<SearchDto>(objectId);
        }

        public async Task UpdateRecord(SearchDto record)
        {
            try
            {
                await _index.SaveObjectAsync(record);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Couldn't update the record to the Algolia", ex.Message);
            }
        }
    }
}
