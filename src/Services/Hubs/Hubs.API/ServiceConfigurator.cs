using Hubs.API.Database;
using Service;

namespace Hubs.API;

public class ServiceConfigurator : ServiceConfiguratorBase<HubsDatabaseContext>
{
    protected override string MigrationsAssembly => "Hubs.API";

    protected override void ConfigureServices(WebApplicationBuilder builder)
    {
        ConfigureManagers(builder);
        ConfigureLocalServices(builder);
    }

    private static void ConfigureManagers(WebApplicationBuilder builder)
    {
    }

    private static void ConfigureLocalServices(WebApplicationBuilder builder)
    {
    }
}