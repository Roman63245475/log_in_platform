using be;

namespace api;

// password is intentionally not editable
public class EditUserDTO
{
    public string username { get; set; }
    public UserRole role { get; set; }
}
