using Microsoft.AspNetCore.Http;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto, HttpResponse response);
        Task<string?> RefreshToken(HttpRequest request, HttpResponse response);
        Task<bool> LogOut(HttpContext httpContext);
    }
}
