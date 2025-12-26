using Hubs.API.Managers;
using Hubs.API.Models.Convertors;
using Hubs.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Extensions;
using Types.Extensions;
using ControllerBase = Service.ControllerBase;

namespace Hubs.API.Controllers;

[ApiController]
[Route("channel/{hubId:guid}")]
[Authorize]
public class ChannelController(ChannelManager channelManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid hubId, [FromBody] NewChannelDto channelDto)
    {
        if (hubId.IsEmpty())
        {
            return BadRequest("Invalid hubId");
        }
        
        var result = await channelManager.CreateAsync(User.GetUserId(), hubId, channelDto);
        return result.Ok ? Ok(result.Value.ToDto()) : OptionResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetForHubAsync([FromRoute] Guid hubId)
    {
        var result = await channelManager.ListForHubAsync(hubId, User.GetUserId());
        return result.Ok ? Ok(result.Value.ToDto()) : OptionResult(result);
    }
}