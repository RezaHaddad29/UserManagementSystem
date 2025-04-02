namespace UserManagement.Application.DTOs
{
    public record RegisterDto(string FirstName, string LastName, string Email, string Password);
    public record LoginDto(string Email, string Password);
    public record RefreshTokenDto(int userId, string refreshToken, DateTime expiryTime);
}
