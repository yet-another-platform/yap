using Hubs.Domain.Mq.Senders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Service.Extensions;
using Types.Types;

namespace RealTime.Hubs;

[Authorize]
public class ChatHub(GetHubsAndChannelsForUserSender getHubsAndChannelsForUserSender) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.GetUserId();
        if (userId is null)
        {
            return;
        }

        var response = await getHubsAndChannelsForUserSender.SendAsync(userId.Value, new CorrelationId(), Context.ConnectionAborted);
        if (Context.ConnectionAborted.IsCancellationRequested || !response.Ok)
        {
            return;
        }

        foreach (var hubId in response.Value.HubIds)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"hub:{hubId}", Context.ConnectionAborted);
        }

        foreach (var hubId in response.Value.ChannelIds)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"channel:{hubId}", Context.ConnectionAborted);
        }
    }
}