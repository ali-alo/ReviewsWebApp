using ReviewsWebApp.Data;
using ReviewsWebApp.DTOs;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task MakeAdmin(string userId);
        Task RemoveAdminRights(string userId);
        Task<bool> LikeReview(string userId, int reviewId);
        Task<bool> DislikeReview(string userId, int reviewId);
        Task BlockUser(string userId);
        Task UnblockUser(string userId);
        Task<ApplicationUser?> GetUserWithLikesById(string userId);
        Task<UserDto?> GetUserDto(string userId);
        Task<List<UserListDto>> GetAllUsers();
        Task<List<UserListDto>> SearchUsers(string searchQuery);
        Task<List<string>> GetUserRoles(string userId);
    }
}
