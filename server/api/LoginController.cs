using be;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api;

//[ApiController]
//[Route("")]
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
        
        return Ok(await userService.login(new User { username = user_dto.username, password = user_dto.password }));
    }
    [AllowAnonymous]
    [HttpPost("register")]
    [Produces("application/json")]
    public IActionResult register([FromBody] UserDTO user_dto)
    {
        userService.register(new User{username = user_dto.username, password = user_dto.password});
        return Ok();
    }
}
