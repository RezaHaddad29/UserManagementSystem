using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;
using static UserManagement.Application.DTOs.UserDTOs;

namespace UserManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task UpdateRefreshTokenAsync(RefreshTokenDto refreshToken);
        Task RemoveRefreshTokenAsync(int userId);
        Task<bool> SaveChangesAsync();
    }
}
