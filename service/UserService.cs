using be;
using db;
using LinqToDB.Async;
using LinqToDB.Internal.DataProvider.Firebird;
using Microsoft.AspNetCore.Identity;

namespace service;

public class UserService
{
    private Database db;

    public UserService(Database db)
    {
        this.db = db;
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
        return user.username;
    }

    public async Task<List<ClientUserDTO>> getAllUsers()
    {
        var users = await db.users.ToListAsync();
        return users.Select(u => new ClientUserDTO{id = u.id, username = u.username}).ToList();
    }
}