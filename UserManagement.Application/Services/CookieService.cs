using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.Services
{
    public class CookieService : ICookieService
    {
        public void SetRefreshToken(HttpResponse response, string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        public string? GetRefreshToken(HttpRequest request)
        {
            request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            return refreshToken;
        }

        public void RemoveRefreshToken(HttpResponse response)
        {
            response.Cookies.Delete("refreshToken");
        }
    }

}
