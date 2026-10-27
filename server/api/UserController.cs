using be;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api;

public class UserController
{
    
    private UserService userService;

    public UserController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpGet(nameof(users))]
    public async Task<List<ClientUserDTO>> users()
    {
        return await userService.getAllUsers();
    }
}
