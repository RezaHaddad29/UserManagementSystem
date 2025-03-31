using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(User user);
        Task<string> LoginAsync(string email, string password);
    }
}
