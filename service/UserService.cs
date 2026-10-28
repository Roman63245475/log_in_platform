using be;
using be.Authorization;
using db;
using LinqToDB;
using LinqToDB.Async;
using Microsoft.AspNetCore.Identity;

namespace service;

public class UserService
{
    private Database db;
    private readonly IAuthorizationService authorizationService;

    public UserService(Database db, IAuthorizationService authorizationService)
    {
        this.db = db;
        this.authorizationService = authorizationService;
    }

    public async Task<string> login(User user)
    {
        var usr = await db.users.FirstOrDefaultAsync(u => u.username == user.username);
        if (usr == null)
        {
          throw new InvalidDataException("Check if email and password are correct");
        }

        var password_hasher = new PasswordHasher<User>();
        PasswordVerificationResult res = password_hasher.VerifyHashedPassword(usr, usr.password, user.password);
        if (res == PasswordVerificationResult.Failed)
        {
            throw new InvalidDataException("Check if email and password are correct");
        }
        
        return authorizationService.GenerateJwtToken(usr);
    }

    public async Task<List<ClientUserDTO>> getAllUsers()
    {
        var users = await db.users.ToListAsync();
        return users.Select(u => new ClientUserDTO{id = u.id, username = u.username}).ToList();
    }

    public void register(User user)
    {
        
        if (user.username == null)
        {
            throw new InvalidDataException("username is required");
        }

        if (user.password == null)
        {
            throw new InvalidDataException("Password is required");
        }
        
        var password_hasher = new PasswordHasher<User>();
        user.password = password_hasher.HashPassword(user, user.password);
        db.Insert(user);
    }
}
