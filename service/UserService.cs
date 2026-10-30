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

    public async Task<List<ClientUserDTO>> getAllUsers(UserRole role)
    {
        if (role == UserRole.Admin)
        {
            return await db.users.Select(u => new ClientUserDTO{id = u.id, username = u.username, role = u.role}).ToListAsync();
        }
        else
        {
            return await db.users.Where(u => u.role == UserRole.User).Select(u => new ClientUserDTO
                { id = u.id, username = u.username, role = u.role }).ToListAsync();
        }
    }
        
    

    // Admin manages everyone, SuperUser manages only regular users, User manages nobody
    private static bool CanManage(UserRole actor, UserRole target) =>
        actor == UserRole.Admin ||
        (actor == UserRole.SuperUser && target == UserRole.User);

    public void register(string username, string rawPassword, UserRole role, UserRole creatorRole)
    {
        if (!Enum.IsDefined(role))
        {
            throw new InvalidDataException("Unknown role");
        }

        if (!CanManage(creatorRole, role))
        {
            throw new UnauthorizedAccessException("Not allowed to create a user with this role");
        }

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
            role = role
        };
        
        var password_hasher = new PasswordHasher<User>();
        string hashed_pass = password_hasher.HashPassword(newUser, rawPassword);
        newUser.password_hash = hashed_pass;
        db.Insert(newUser);
    }

    // public async Task<List<ClientUserDTO>> GetAllUsersWithRole(string role) {
    //     return await db.users.Join(db.roles, u => u.role, r => r.id,
    //         (u, r) => new ClientUserDTO { id = u.id, username = u.username, role = r.rol}).Where(dto => dto.role_name == role).ToListAsync();
    // }

    public async Task DeleteUser(int id, UserRole deleterRole) {
        var target = await db.users.FirstOrDefaultAsync(u => u.id == id);
        if (target == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        if (!CanManage(deleterRole, target.role))
        {
            throw new UnauthorizedAccessException("Not allowed to delete this user");
        }

        await db.users.DeleteAsync(u => u.id == id);
    }
}