using Microsoft.EntityFrameworkCore;
using UserManagement.Application.DTOs;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Data;
using static UserManagement.Application.DTOs.UserDTOs;

namespace UserManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            // Update بدون SaveChangesAsync؛ SaveChangesAsync توسط سرویس فراخوانی می‌شود.
            _context.Users.Update(user);
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            // استفاده از AsNoTracking برای بهبود عملکرد خواندن
            return await _context.Users
                .AsNoTracking()
                .Select(user => new UserDto(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.IsActive
                ))
                .ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            // استفاده از FindAsync اگر کلید اصلی باشد
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task UpdateRefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var user = await _context.Users.FindAsync(refreshTokenDto.userId);
            if (user != null)
            {
                user.RefreshToken = refreshTokenDto.refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenDto.expiryTime;
            }
        }

        public async Task RemoveRefreshTokenAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
