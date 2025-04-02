using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Interfaces;
using static UserManagement.Application.DTOs.UserDTOs;

namespace UserManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log error (مثلاً با ILogger)
                return StatusCode(500, new { message = "An error occurred while fetching users." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update-role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequest request)
        {
            try
            {
                var result = await _userService.UpdateUserRoleAsync(request);
                if (!result)
                    return NotFound(new { message = "User not found or update failed" });

                return Ok(new { message = "User role updated successfully" });
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, new { message = "An error occurred while updating user role." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(userId, request);
                if (!result)
                    return NotFound(new { message = "User not found" });

                return Ok(new { message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, new { message = "An error occurred while updating user." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(userId);
                if (!result)
                    return NotFound(new { message = "User not found" });

                return Ok(new { message = "User deactivated successfully" });
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, new { message = "An error occurred while deleting user." });
            }
        }
    }
}
