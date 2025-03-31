using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using System.Threading.Tasks;

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

        // POST auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var result = await _authService.RegisterAsync(user);
            if (result == "User registered successfully.")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // POST auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var result = await _authService.LoginAsync(loginUser.Email, loginUser.PasswordHash);
            if (result == "Login successful.")
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }
    }
}
