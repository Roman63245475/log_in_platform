using System.Security.Claims;
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

    [HttpGet("users_role")]
    public async Task<List<ClientUserDTO>> users_role([FromQuery] string role) {
        return await userService.GetAllUsersWithRole(role);
    }
    
    [Authorize]
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> delete_user([FromRoute] int id) {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        await userService.DeleteUser(id, userRole);
        return Ok();
    }
    
    
}
