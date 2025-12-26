using Hubs.API.DatabaseServices.Interfaces;
using Hubs.Domain.Constants;
using Hubs.Domain.Mq.Requests;
using Hubs.Domain.Mq.Results;
using Mq;
using RabbitMQ.Client;
using Types.Extensions;

namespace Hubs.API.Mq.Consumers;

public class GetHubsAndChannelsForUserConsumer(
    ILogger<GetHubsAndChannelsForUserConsumer> logger,
    IConnection connection,
    IServiceProvider serviceProvider)
    : ConsumerBase<GetHubsAndChannelsForUserRequest, GetHubsAndChannelsForUserResult>(logger, connection)
{
    protected override string QueueName => QueueNames.GetHubsAndChannelsForUser;

    public override async Task<GetHubsAndChannelsForUserResult> ConsumeAsync(GetHubsAndChannelsForUserRequest request)
    {
        var scope = serviceProvider.CreateAsyncScope();
        var hubDatabaseService = scope.ServiceProvider.GetRequiredService<IHubDatabaseService>();
        var channelDatabaseService = scope.ServiceProvider.GetRequiredService<IChannelDatabaseService>();

        var hubs = await hubDatabaseService.ListJoinedForUser(request.UserId);
        if (hubs.Count == 0)
        {
            return new GetHubsAndChannelsForUserResult();
        }

        var hubIds = hubs.Select(h => h.Id).ToCheckedList();
        var channels = await channelDatabaseService.ListForHubsAndUser(hubIds, request.UserId);
        return new GetHubsAndChannelsForUserResult
        {
            HubIds = hubIds,
            ChannelIds = channels.Select(c => c.Id).ToCheckedList()
        };
    }
}