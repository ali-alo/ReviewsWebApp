using ReviewsWebApp.Data;
using ReviewsWebApp.DTOs;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task MakeAdmin(string username);
        Task<bool> LikeReview(string userId, int reviewId);
        Task<bool> DislikeReview(string userId, int reviewId);
        Task BlockUser(string userId);
        Task<ApplicationUser?> GetUserWithLikesById(string userId);
        Task<UserDto?> GetUserDto(string userId);
    }
}
