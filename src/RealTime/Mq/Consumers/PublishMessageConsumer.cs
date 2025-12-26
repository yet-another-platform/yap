using Hubs.Domain.DataTransferObjects;
using Microsoft.AspNetCore.SignalR;
using Mq;
using Mq.Results;
using RabbitMQ.Client;
using RealTime.Domain.Constants;
using RealTime.Hubs;

namespace RealTime.Mq.Consumers;

public class PublishMessageConsumer(ILogger<PublishMessageConsumer> logger, IConnection connection, IHubContext<ChatHub> chatHubContext) : ConsumerBase<MessageDto, BoolResult>(logger, connection)
{
    protected override string QueueName => QueueNames.PublishMessage;
    public override async Task<BoolResult> ConsumeAsync(MessageDto request)
    {
        await chatHubContext.Clients.Group($"channel:{request.ChannelId}").SendAsync("ReceiveMessage", request);
        return new BoolResult();
    }
}