using Microsoft.Extensions.DependencyInjection;
using Mq;
using Mq.Interfaces;

namespace Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSender<TRequest, TResponse, TSender>(this IServiceCollection services)
        where TSender : SenderBase<TRequest, TResponse>
    {
        services.AddSingleton<ISenderBase, TSender>();
        services.AddSingleton<TSender>(sp => sp.GetServices<ISenderBase>().OfType<TSender>().First());
        return services;
    }

    public static IServiceCollection AddConsumer<TRequest, TResponse, TConsumer>(this IServiceCollection services) where TConsumer : ConsumerBase<TRequest, TResponse>
    {
        services.AddSingleton<IConsumerBase, TConsumer>();
        services.AddSingleton<TConsumer>(sp => sp.GetServices<IConsumerBase>().OfType<TConsumer>().First());
        return services;
    }
}