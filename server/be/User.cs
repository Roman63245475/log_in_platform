namespace be;

public class User
{
    public int id { get; set; }
    public string username { get; set; }
    public string password_hash { get; set; }
    
    public UserRole role_id { get; set; } = UserRole.User;

    public User()
    {
        
    }
    
    public User(int id, string username, string password, UserRole role_id)
    {
        this.id = id;
        this.username = username;
        this.password_hash = password;
        this.role_id = role_id;
    }
}
