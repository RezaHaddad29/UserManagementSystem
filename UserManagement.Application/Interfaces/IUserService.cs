using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;
using static UserManagement.Application.DTOs.UserDTOs;

namespace UserManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsers();
        Task<bool> UpdateUserRoleAsync(UpdateUserRoleRequest request);
        Task<bool> UpdateUserAsync(int userId, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(int userId);
    }
}
