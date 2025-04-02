namespace UserManagement.Application.DTOs
{
    public class UserDTOs
    {
        public record UserDto(int Id, string FirstName, string LastName, string Email, bool IsActive);
        public record UpdateUserRoleRequest(int userId, string newRole);
        public record UpdateUserRequest(string FirstName, string LastName, bool IsActive);
    }
}
