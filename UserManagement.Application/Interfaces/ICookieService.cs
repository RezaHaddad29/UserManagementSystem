using Microsoft.AspNetCore.Http;

namespace UserManagement.Application.Interfaces
{
    public interface ICookieService
    {
        void SetRefreshToken(HttpResponse response, string refreshToken);
        string? GetRefreshToken(HttpRequest request);
        void RemoveRefreshToken(HttpResponse response);
    }
}
