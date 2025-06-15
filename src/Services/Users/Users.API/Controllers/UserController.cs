using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Types.Extensions;
using Users.API.Managers;
using Users.API.Models.Convertors;
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

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        var loginResult = await userManager.LoginAsync(loginDto);
        return OptionResult(loginResult);
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentAsync()
    {
        var userId = User.GetUserId();
        var result = await userManager.GetAsync(userId);
        return result.Ok ? Ok(result.Value.ToFullDto()) : OptionResult(result);
    }
}