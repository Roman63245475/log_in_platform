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
    
}
