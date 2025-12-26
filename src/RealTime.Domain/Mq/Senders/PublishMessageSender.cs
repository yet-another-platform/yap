using Hubs.Domain.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Mq;
using Mq.Results;
using RabbitMQ.Client;
using RealTime.Domain.Constants;
using Types.Types;

namespace RealTime.Domain.Mq.Senders;

public class PublishMessageSender(ILogger<PublishMessageSender> logger, IConnection connection) : SenderBase<MessageDto, BoolResult>(logger, connection)
{
    public override string QueueName => QueueNames.PublishMessage;

    public async Task<bool> PublishAsync(MessageDto messageDto, CorrelationId correlationId, CancellationToken cancellationToken = default)
    {
        var result = await SendInternalAsync(messageDto, correlationId, cancellationToken);
        if (!result.Ok || !result.Value.Value)
        {
            logger.LogError("Failed to publish message");
            return false;
        }
        
        return true;
    }
}