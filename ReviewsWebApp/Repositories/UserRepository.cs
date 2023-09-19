using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;

namespace ReviewsWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task BlockUser(string userId)
        {
            throw new NotImplementedException();
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

        public Task MakeAdmin(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser?> GetUserWithLikesById(string userId) =>
            await _context.Users.Include(u => u.LikedReviews).FirstOrDefaultAsync(u => u.Id == userId);

        public async Task<UserDto?> GetUserDto(string userId) =>
            await _context.Users
            .Where(user => userId == user.Id)
            .Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
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
    }
}
