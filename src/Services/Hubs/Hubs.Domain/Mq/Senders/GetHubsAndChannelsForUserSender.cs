using Hubs.Domain.Constants;
using Hubs.Domain.Mq.Requests;
using Hubs.Domain.Mq.Results;
using Microsoft.Extensions.Logging;
using Mq;
using RabbitMQ.Client;
using Types.Types;
using Types.Types.Option;

namespace Hubs.Domain.Mq.Senders;

public class GetHubsAndChannelsForUserSender(ILogger<GetHubsAndChannelsForUserSender> logger, IConnection connection)
    : SenderBase<GetHubsAndChannelsForUserRequest, GetHubsAndChannelsForUserResult>(logger, connection)
{
    public override string QueueName => QueueNames.GetHubsAndChannelsForUser;

    public async Task<Option<GetHubsAndChannelsForUserResult>> SendAsync(GuidChecked userId, CorrelationId correlationId, CancellationToken cancellationToken = default)
    {
        var result = await SendInternalAsync(new GetHubsAndChannelsForUserRequest { UserId = userId }, correlationId, cancellationToken);
        if (!result.Ok)
        {
            logger.LogError("Couldn't get hubs and channels for the user: {UserId} | {Message} | {CorrelationId}", userId, result.Error.Message, correlationId);
        }

        return result;
    }
}