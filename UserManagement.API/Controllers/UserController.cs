using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Interfaces;
using static UserManagement.Application.DTOs.UserDTOs;

namespace UserManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) { _userService = userService; }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update-role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequest request)
        {
            var result = await _userService.UpdateUserRoleAsync(request);
            if (!result) return BadRequest(new { message = "User not found or update failed" });

            return Ok(new { message = "User role updated successfully" });
        }
    }
}
