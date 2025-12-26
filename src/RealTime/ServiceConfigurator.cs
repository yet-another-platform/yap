using Hubs.Domain.DataTransferObjects;
using Hubs.Domain.Mq.Requests;
using Hubs.Domain.Mq.Results;
using Hubs.Domain.Mq.Senders;
using Mq.Results;
using RealTime.Mq.Consumers;
using Service;
using Service.Extensions;

namespace RealTime;

public class ServiceConfigurator : ServiceConfiguratorBase
{
    protected override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
        builder.Services.AddSender<GetHubsAndChannelsForUserRequest, GetHubsAndChannelsForUserResult, GetHubsAndChannelsForUserSender>();
        builder.Services.AddConsumer<MessageDto, BoolResult, PublishMessageConsumer>();
    }
}