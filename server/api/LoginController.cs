using be;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api;

public class LoginController
{
    
    private UserService userService;

    public LoginController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpPost(nameof(login))]
    [Produces("application/json")]
    public async Task<string> login([FromBody] UserDTO user_dto)
    {
        return await userService.login(user_dto.username, user_dto.password);
    }
}
