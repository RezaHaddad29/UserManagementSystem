using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using System.Threading.Tasks;

namespace UserManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> RegisterAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return "User with this email already exists.";
            }

            await _userRepository.CreateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return "User registered successfully.";
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || user.PasswordHash != password)
            {
                return "Invalid email or password.";
            }

            return "Login successful.";
        }
    }
}
