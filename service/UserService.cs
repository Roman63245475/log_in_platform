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

    public async Task<string> login(string username, string rawPassword)
    {
        var usr = await db.users.FirstOrDefaultAsync(u => u.username == username);
        if (usr == null)
        {
          throw new InvalidDataException("Check if email and password are correct");
        }

        var password_hasher = new PasswordHasher<User>();
        PasswordVerificationResult res = password_hasher.VerifyHashedPassword(usr, usr.password_hash, rawPassword);
        if (res == PasswordVerificationResult.Failed)
        {
            throw new InvalidDataException("Check if email and password are correct");
        }
        return authorizationService.GenerateJwtToken(usr);
    }

    public async Task<List<ClientUserDTO>> getAllUsers()
    {
        return await db.users.Join(db.roles, u => u.role_id, r => r.id, (u, r) =>  new ClientUserDTO { id = u.id, username = u.username, role_name = r.role_name} ).ToListAsync();
    }

    public void register(string username, string rawPassword)
    {
        
        if (username == null)
        {
            throw new InvalidDataException("username is required");
        }

        if (rawPassword == null)
        {
            throw new InvalidDataException("Password is required");
        }

        var newUser = new User {
            username = username,
            role_id = (int)UserRole.User
        };
        
        var password_hasher = new PasswordHasher<User>();
        string hashed_pass = password_hasher.HashPassword(newUser, rawPassword);
        newUser.password_hash = hashed_pass;
        db.Insert(newUser);
    }

    public async Task<List<ClientUserDTO>> GetAllUsersWithRole(string role) {
        return await db.users.Join(db.roles, u => u.role_id, r => r.id,
            (u, r) => new ClientUserDTO { id = u.id, username = u.username, role_name = r.role_name}).Where(dto => dto.role_name == role).ToListAsync();
    }
}