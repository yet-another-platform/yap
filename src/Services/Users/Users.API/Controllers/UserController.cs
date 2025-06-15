using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.API.Managers;
using Users.Domain.DataTransferObjects;
using ControllerBase = Service.ControllerBase;

namespace Users.API.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public class UserController(UserManager userManager) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
    {
        var registerResult = await userManager.RegisterAsync(registerDto);
        return OptionResult(registerResult);
    }
}