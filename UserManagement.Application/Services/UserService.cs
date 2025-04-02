using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Application.DTOs;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using static UserManagement.Application.DTOs.UserDTOs;

namespace UserManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            try
            {
                return await _userRepository.GetUsersAsync();
            }
            catch (Exception ex)
            {
                // Log error if necessary
                throw new ApplicationException("Error fetching users.", ex);
            }
        }

        public async Task<bool> UpdateUserRoleAsync(UpdateUserRoleRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(request.userId);
                if (user == null) return false;

                user.Role = request.newRole;
                await _userRepository.UpdateUserAsync(user);
                // SaveChangesAsync در سرویس
                return await _userRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException("Error updating user role.", ex);
            }
        }

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null) return false;

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.IsActive = request.IsActive;

                await _userRepository.UpdateUserAsync(user);
                return await _userRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException("Error updating user.", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null) return false;

                user.IsActive = false;
                await _userRepository.UpdateUserAsync(user);
                return await _userRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException("Error deleting user.", ex);
            }
        }
    }
}
