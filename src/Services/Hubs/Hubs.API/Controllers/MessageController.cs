using Hubs.API.Managers;
using Hubs.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Types.Extensions;
using ControllerBase = Service.ControllerBase;

namespace Hubs.API.Controllers;

[ApiController]
[Route("message")]
[Authorize]
public class MessageController(MessageManager messageManager) : ControllerBase
{
    [HttpPost("{channelId:guid}")]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid channelId, [FromBody] NewMessageDto message)
    {
        if (channelId.IsEmpty())
        {
            return BadRequest("Invalid channelId");
        }
        
        var result = await messageManager.CreateAsync(User.GetUserId(), channelId, message);
        return result.Ok ? Ok(result.Value) : OptionResult(result);
    }
}