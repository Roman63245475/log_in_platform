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
        var user_role = User.FindFirst(ClaimTypes.Role)?.Value;
        var role = Enum.Parse<UserRole>(user_role);
        return await userService.getAllUsers(role);
    }

    // [HttpGet("users_role")]
    // public async Task<List<ClientUserDTO>> users_role([FromQuery] string role) {
    //     return await userService.GetAllUsersWithRole(role);
    // }
    
    [Authorize]
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> delete_user([FromRoute] int id) {
        var user_role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (!Enum.TryParse<UserRole>(user_role, out var deleter_role))
        {
            return Forbid();
        }

        try
        {
            await userService.DeleteUser(id, deleter_role);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        return Ok();
    }
    
    
}
