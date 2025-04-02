using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using System.Threading.Tasks;
using UserManagement.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (result == "User registered successfully.")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto, Response);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _authService.RefreshToken(Request,Response);
            if (result != null)
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            var result = await _authService.LogOut(HttpContext);
            if (result)
            {
                return Ok("Logged out successfully");
            }

            return BadRequest("Logout failed");
        }
    }
}
