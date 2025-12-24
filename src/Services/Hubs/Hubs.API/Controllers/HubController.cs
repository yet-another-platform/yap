using Hubs.API.Managers;
using Hubs.API.Models.Convertors;
using Hubs.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Extensions;
using ControllerBase = Service.ControllerBase;

namespace Hubs.API.Controllers;

[ApiController]
[Route("hub")]
[Authorize]
public class HubController(HubManager hubManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] NewHubDto newHubDto)
    {
        var result = await hubManager.CreateAsync(User.GetUserId(), newHubDto);
        return !result.Ok ? OptionResult(result) : Ok(result.Value.ToDto());
    }

    [HttpGet("joined")]
    public async Task<IActionResult> GetJoined()
    {
        var result = await hubManager.ListJoinedForUser(User.GetUserId());
        return result.Ok ? Ok(result.Value.ToDto()) : OptionResult(result);
    }
}