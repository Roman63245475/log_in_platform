using be;

namespace api;

public class UserDTO
{
    public string username { get; set; }
    public string password { get; set; }
    public UserRole role { get; set; } = UserRole.User;
}
