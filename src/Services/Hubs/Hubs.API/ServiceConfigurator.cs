using Hubs.API.Database;
using Hubs.API.DatabaseServices;
using Hubs.API.DatabaseServices.Interfaces;
using Hubs.API.Managers;
using Service;

namespace Hubs.API;

public class ServiceConfigurator : ServiceConfiguratorBase<HubsDatabaseContext>
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
        // Nothing to configure
    }
}