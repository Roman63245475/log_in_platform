using be;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api;

[ApiController]
[Route("")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserService userService;

    public UserController(UserService userService)
    {
        this.userService = userService;
    }
    [HttpGet("users")]
    public async Task<List<ClientUserDTO>> users()
    {
        return await userService.getAllUsers();
    }
}
