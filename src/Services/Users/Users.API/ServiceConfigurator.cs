using Service;
using Users.API.Database;
using Users.API.DatabaseServices;
using Users.API.DatabaseServices.Interfaces;
using Users.API.Managers;

namespace Users.API;

public class ServiceConfigurator : ServiceConfiguratorWithDatabaseBase<UsersDatabaseContext>
{
    protected override string MigrationsAssembly => "Users.API";

    protected override void ConfigureServices(WebApplicationBuilder builder)
    {
        ConfigureDatabase(builder);
        ConfigureManagers(builder);
    }

    private static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        // Database services
        builder.Services.AddScoped<IUserDatabaseService, UserDatabaseService>();
    }

    private static void ConfigureManagers(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserManager>();
    }
}