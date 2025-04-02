using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Application.DTOs;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ICookieService _cookieService;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, ICookieService cookieService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _cookieService = cookieService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return "User with this email already exists.";
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User()
            {
                PasswordHash = hashedPassword,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };

            await _userRepository.CreateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return "User registered successfully.";
        }

        public async Task<string> LoginAsync(LoginDto dto, HttpResponse response)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            await _userRepository.UpdateRefreshTokenAsync(new RefreshTokenDto(
                userId:user.Id,
                refreshToken:refreshToken,
                expiryTime: DateTime.UtcNow.AddDays(7)));

            await _userRepository.SaveChangesAsync();

            _cookieService.SetRefreshToken(response, refreshToken);


            return accessToken;
        }

        public async Task<string?> RefreshToken(HttpRequest request, HttpResponse response)
        {
            var currentRefreshToken =  _cookieService.GetRefreshToken(request);

            var user = await _userRepository.GetByRefreshTokenAsync(currentRefreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return null; // Token is invalid or expired
            }

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            await _userRepository.UpdateRefreshTokenAsync(new RefreshTokenDto( user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7)));
            await _userRepository.SaveChangesAsync();

            _cookieService.SetRefreshToken(response, newRefreshToken);

            return newAccessToken;
        }

        public async Task<bool> LogOut(HttpContext httpContext)
        {
            var refreshToken = _cookieService.GetRefreshToken(httpContext.Request);

            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return false; // Token is invalid or expired
            }

            await _userRepository.RemoveRefreshTokenAsync(user.Id);
            await _userRepository.SaveChangesAsync();

            _cookieService.RemoveRefreshToken(httpContext.Response);

            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
