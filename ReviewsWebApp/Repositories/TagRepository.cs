using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;
using System.Text.RegularExpressions;

namespace ReviewsWebApp.Repositories
{
    public class TagRepository : ITagRepository
    {
        private static string _tagRegex = @"#[\w\d]+";
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<List<Tag>> GetTagsFromInput(string input)
        {
            var inputTags = GetTagNames(input);
            var existingTags = await _context.Tags
                .Where(tag => inputTags.Contains(tag.Name))
                .ToListAsync();
            var newTagNames = inputTags.Except(existingTags.Select(tag => tag.Name)).ToList();

            // these tags are new, and they will be added to the db when review is saved
            var newTags = newTagNames.Select(tagName => new Tag { Name = tagName }).ToList();
            return existingTags.Concat(newTags).ToList();
        }

        private IEnumerable<string> GetTagNames(string input)
        {
            var matches = GetRegexMatches(input);
            List<string> tags = matches.Cast<Match>()
                .Select(m => m.Value.Trim().ToLower())
                .Distinct()
                .ToList();
            return tags;
        }

        private MatchCollection GetRegexMatches(string input)
        {
            Regex hashtagRegex = new Regex(_tagRegex);
            return hashtagRegex.Matches(input);
        }

        public async Task DeleteTagsWithNoReviews()
        {
            var tagsToDelete = _context.Tags
                .Where(tag => !tag.Reviews.Any())
                .ToList();
            foreach (var tag in tagsToDelete)
                _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}
