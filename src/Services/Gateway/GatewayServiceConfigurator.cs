using Ocelot.DependencyInjection;

namespace Gateway;

public static class GatewayServiceConfigurator
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile(Path.Combine("Configuration", "configuration.json"), false, true)
            .AddJsonFile(Path.Combine("Configuration", $"configuration.{builder.Environment.EnvironmentName}.json"),
                true, true);
        builder.Services.AddOcelot(builder.Configuration);
        return builder;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.UseWebSockets();
        return app;
    }
}