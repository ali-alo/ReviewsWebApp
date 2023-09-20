using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Areas.Identity;
using ReviewsWebApp.Data;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Repositories.Interfaces;
using System.Collections.Immutable;
using System.Data;

namespace ReviewsWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task BlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return;
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;
            await _userManager.UpdateAsync(user);
        }

        public async Task UnblockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return;
            user.LockoutEnabled = false;
            user.LockoutEnd = DateTimeOffset.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> DislikeReview(string userId, int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            var user = await GetUserWithLikesById(userId);
            if (review == null || user == null || !user.LikedReviews.Contains(review))
                return false;
            user.LikedReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> LikeReview(string userId, int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            var user = await GetUserWithLikesById(userId);
            if (review == null || user == null || user.LikedReviews.Contains(review))
                return false;
            user.LikedReviews.Add(review);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task MakeAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return;
            if (await _roleManager.RoleExistsAsync(ApplicationRoleTypes.Admin))
                await _userManager.AddToRoleAsync(user, ApplicationRoleTypes.Admin);

        }

        public async Task RemoveAdminRights(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return;
            if (await _userManager.IsInRoleAsync(user, ApplicationRoleTypes.Admin))
                await _userManager.RemoveFromRoleAsync(user, ApplicationRoleTypes.Admin);
        }

        public async Task<ApplicationUser?> GetUserWithLikesById(string userId) =>
            await _context.Users.Include(u => u.LikedReviews).FirstOrDefaultAsync(u => u.Id == userId);

        public async Task<UserDto?> GetUserDto(string userId)
        {
            var user =  await _context.Users
                .Where(user => userId == user.Id)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsBlocked = IsUserBlocked(user),
                    Reviews = user.Reviews.Select(r => new ReviewDetailsDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        MarkdownText = r.MarkdownText,
                        Grade = r.Grade,
                        CreatorFirstName = r.ApplicationUser == null ? null : r.ApplicationUser.FirstName,
                        CreatorLastName = r.ApplicationUser == null ? null : r.ApplicationUser.LastName,
                        CreatorId = r.ApplicationUser == null ? null : r.ApplicationUser.Id,
                        ReviewItemNameEn = r.ReviewItem.NameEn,
                        ReviewItemNameRu = r.ReviewItem.NameRu,
                        ReviewItemId = r.ReviewItem.Id,
                        ReviewItemGroupNameEn = r.ReviewItem.ReviewGroup.NameEn,
                        UsersIdWhoLiked = r.UsersWhoLiked.Select(u => u.Id).ToList()
                    }).ToList(),
                    LikedReviews = user.LikedReviews.Select(r => new ReviewDetailsDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        MarkdownText = r.MarkdownText,
                        Grade = r.Grade,
                        CreatorFirstName = r.ApplicationUser == null ? null : r.ApplicationUser.FirstName,
                        CreatorLastName = r.ApplicationUser == null ? null : r.ApplicationUser.LastName,
                        CreatorId = r.ApplicationUser == null ? null : r.ApplicationUser.Id,
                        ReviewItemNameEn = r.ReviewItem.NameEn,
                        ReviewItemNameRu = r.ReviewItem.NameRu,
                        ReviewItemId = r.ReviewItem.Id,
                        ReviewItemGroupNameEn = r.ReviewItem.ReviewGroup.NameEn,
                        UsersIdWhoLiked = r.UsersWhoLiked.Select(u => u.Id).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync(r => r.Id == userId);

            if (user != null)
                user.IsAdmin = await IsUserAdmin(user.Id);
            return user;
        }

        public async Task<List<UserListDto>> GetAllUsers()
        {
            var users = await GetUsersWithQuery();
            foreach (var user in users)
                await LoadUserStats(user);
            return users;
        }

        public async Task<List<UserListDto>> SearchUsers(string searchQuery)
        {
            var users = await SearchUsersWithQuery(searchQuery);
            foreach (var user in users)
                await LoadUserStats(user);
            return users;
        }

        private async Task<List<UserListDto>> GetUsersWithQuery() =>
            await (
                from u in _context.Users
                orderby u.FirstName, u.LastName
                select new UserListDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsBlocked = IsUserBlocked(u),
                }
            ).ToListAsync();

        private async Task<List<UserListDto>> SearchUsersWithQuery(string searchQuery) =>
         await (
            from u in _context.Users
            where (u.FirstName + " " + u.LastName).ToLower()
                .Contains(searchQuery.ToLower().Trim())  // search by full name
            orderby u.FirstName, u.LastName
            select new UserListDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsBlocked = IsUserBlocked(u),
            }
        ).ToListAsync();

        private async Task LoadUserStats(UserListDto user)
        {
            var userReviews = await _context.Reviews
                    .Where(r => r.CreatorId == user.Id)
                    .Include(r => r.UsersWhoLiked)
                    .ToListAsync();
            user.ReviewsCount = userReviews.Count;
            user.LikesAmount = userReviews.Any() ? userReviews.SelectMany(r => r.UsersWhoLiked).Count() : 0;
            user.ReviewsAverageRating = userReviews.Any() ? userReviews.Average(r => r.Grade) : 0;
            user.IsAdmin = await IsUserAdmin(user.Id);
        }

        private static bool IsUserBlocked(ApplicationUser user) => 
            user.LockoutEnabled && user.LockoutEnd > DateTimeOffset.UtcNow;
        
        private async Task<bool> IsUserAdmin(string userId)
        {
            var query = from u in _context.UserRoles
                        join ur in _context.UserRoles on u.UserId equals ur.UserId
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where u.UserId == userId
                        select r.Name == ApplicationRoleTypes.Admin;

            return await query.AnyAsync();
        }

        public async Task<List<string>> GetUserRoles(string userId) =>
            await (
                from u in _context.UserRoles
                join ur in _context.UserRoles on u.UserId equals ur.UserId
                join r in _context.Roles on ur.RoleId equals r.Id
                where u.UserId == userId
                select r.Name).ToListAsync();
    }
}
