using ReviewsWebApp.Data;
using ReviewsWebApp.DTOs;

namespace ReviewsWebApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task MakeAdmin(string userId);
        Task RemoveAdminRights(string userId);
        Task<bool> ToggleReviewLike(string userId, int reviewId);
        Task BlockUser(string userId);
        Task UnblockUser(string userId);
        Task<ApplicationUser?> GetUserWithLikesById(string userId);
        Task<UserDto?> GetUserDto(string userId);
        Task<List<UserListDto>> GetAllUsers();
        Task<List<UserListDto>> SearchUsers(string searchQuery);
    }
}
