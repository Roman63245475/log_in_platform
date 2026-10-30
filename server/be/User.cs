namespace be;

public class User
{
    public int id { get; set; }
    public string username { get; set; }
    public string password_hash { get; set; }
    
    public UserRole role { get; set; }
    
    public User()
    {
        
    }
    
    public User(int id, string username, string password, UserRole role)
    {
        this.id = id;
        this.username = username;
        this.password_hash = password;
        this.role = role;
    }
}
