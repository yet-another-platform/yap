using Hubs.API.Database;
using Hubs.API.DatabaseServices;
using Hubs.API.DatabaseServices.Interfaces;
using Hubs.API.Managers;
using Hubs.API.Mq.Consumers;
using Hubs.Domain.DataTransferObjects;
using Hubs.Domain.Mq.Requests;
using Hubs.Domain.Mq.Results;
using Mq.Results;
using RealTime.Domain.Mq.Senders;
using Service;
using Service.Extensions;

namespace Hubs.API;

public class ServiceConfigurator : ServiceConfiguratorWithDatabaseBase<HubsDatabaseContext>
{
    protected override string MigrationsAssembly => "Hubs.API";

    protected override void ConfigureServices(WebApplicationBuilder builder)
    {
        ConfigureManagers(builder);
        ConfigureDatabaseServices(builder);
        ConfigureLocalServices(builder);
    }

    private static void ConfigureManagers(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<HubManager>();
        builder.Services.AddScoped<ChannelManager>();
        builder.Services.AddScoped<MessageManager>();
    }

    private static void ConfigureDatabaseServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IHubDatabaseService, HubDatabaseService>();
        builder.Services.AddScoped<IChannelDatabaseService, ChannelDatabaseService>();
        builder.Services.AddScoped<IMessageDatabaseService, MessageDatabaseService>();
    }

    private static void ConfigureLocalServices(WebApplicationBuilder builder)
    {
        builder.Services.AddConsumer<GetHubsAndChannelsForUserRequest, GetHubsAndChannelsForUserResult, GetHubsAndChannelsForUserConsumer>();
        builder.Services.AddSender<MessageDto, BoolResult, PublishMessageSender>();
    }
}