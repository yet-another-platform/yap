using System.Threading.Tasks;
using Client.Models.Interfaces;
using Hubs.Domain.DataTransferObjects;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Client.Models;

public class RealtimeManager(ILogger<RealtimeManager> logger, IAuthSession authSession)
{
    private HubConnection? _hubConnection = null;

    public async Task ConnectAsync()
    {
        if (!authSession.IsLoggedIn)
        {
            logger.LogError("Tried to connect but user is not logged in");
            return;
        }

        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:40000/rt/chat",
                (options) => { options.AccessTokenProvider = async () => await Task.FromResult(authSession.Token); })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.Reconnecting += (ex) =>
        {
            logger.LogInformation(ex, "Reconnecting...");
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += (connectionId) =>
        {
            logger.LogInformation("Reconnected with connection ID: {ConnectionId}", connectionId);
            return Task.CompletedTask;
        };

        _hubConnection.On<MessageDto>("ReceiveMessage", ReceiveMessage);
        await _hubConnection.StartAsync();
    }

    private async Task ReceiveMessage(MessageDto message)
    {
        logger.LogInformation("Received message in channel {ChannelId} by user {UserId} at {CreatedAt}: {Message}", message.ChannelId, message.UserId,
            message.Created.ToString("g"), message.Content);
    }
}