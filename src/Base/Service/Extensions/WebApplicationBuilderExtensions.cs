using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Service.Interfaces;
using Service.Jobs;
using Types.Exceptions;

namespace Service.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static async Task<WebApplicationBuilder> ConfigureAsync<TConfigurator>(this WebApplicationBuilder builder) where TConfigurator : IServiceConfigurator, new()
    {
        var configuration = builder.Configuration;
        var mqConfiguration = configuration.GetRequiredSection("MQ");
        var mqConnectionFactory = new ConnectionFactory { HostName = mqConfiguration["HostName"] ?? throw new ConfigurationException("MQ connection string not found") };
        var connection = await mqConnectionFactory.CreateConnectionAsync();
        builder.Services.AddSingleton(connection);
        
        builder.Services.AddHostedService<StartSendersAndConsumers>();
        var configurator = new TConfigurator();
        return configurator.Configure(builder);
    }
}