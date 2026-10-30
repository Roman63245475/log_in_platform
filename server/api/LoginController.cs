using System.Security.Claims;
using be;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api;

public class LoginController : ControllerBase
{
    private readonly UserService userService;

    public LoginController(UserService userService)
    {
        this.userService = userService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    [Produces("application/json")]
    public async Task<ActionResult<string>> login([FromBody] UserDTO user_dto)
    {
        return Ok(await userService.login(user_dto.username, user_dto.password));
    }
    
    [Authorize]
    [HttpPost("register")]
    [Produces("application/json")]
    public IActionResult register([FromBody] UserDTO user_dto)
    {
        var user_role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (!Enum.TryParse<UserRole>(user_role, out var creator_role))
        {
            return Forbid();
        }

        try
        {
            userService.register(user_dto.username, user_dto.password, user_dto.role, creator_role);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        return Ok();
    }
}
