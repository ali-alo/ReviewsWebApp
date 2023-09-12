using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Models;
using ReviewsWebApp.Services.Interfaces;
using System.Text.RegularExpressions;

namespace ReviewsWebApp.Services
{
    public class TagService : ITagService
    {
        private static string _tagRegex = @"#[\w\d]+";
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.AsNoTracking().ToListAsync();
        }

        public List<Tag> GetTagsFromString(string input)                                                                                                                                                                                                                 
        {
            var matches = GetRegexMatches(input);
            List<Tag> tags = matches.Cast<Match>()
                .Select(m => new Tag { Name = m.Value.Trim().ToLower() })
                .ToList();
            return tags;
        }

        private MatchCollection GetRegexMatches(string input)
        {
            Regex hashtagRegex = new Regex(_tagRegex);
            return hashtagRegex.Matches(input);
        }
    }
}
