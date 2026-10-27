namespace be;

public class RegularUser
{
    public int id { get; set; }
    public string username { get; set; }
    public string password { get; set; }

    public RegularUser()
    {
        
    }
    
    public RegularUser(int id, string username, string password)
    {
        this.id = id;
        this.username = username;
        this.password = password;
    }
}
