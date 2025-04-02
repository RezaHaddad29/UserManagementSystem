using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using static UserManagement.Application.DTOs.UserDTOs;

namespace UserManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) { _userRepository = userRepository; }

        public async Task<List<User>> GetUsers()
        {
           return await _userRepository.GetUsersAsync();
        }

        public async Task<bool> UpdateUserRoleAsync(UpdateUserRoleRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(request.userId);
            if (user == null) return false;

            user.Role = request.newRole;
            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }
    }
}
