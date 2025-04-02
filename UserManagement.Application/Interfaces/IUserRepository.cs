using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
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
