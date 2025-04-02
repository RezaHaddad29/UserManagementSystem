namespace UserManagement.Application.DTOs
{
    public class UserDTOs
    {
        public record UpdateUserRoleRequest(int userId, string newRole);
    }
}
