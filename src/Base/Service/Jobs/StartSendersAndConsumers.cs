using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mq.Interfaces;

namespace Service.Jobs;

public class StartSendersAndConsumers(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    { 
        var scope = serviceProvider.CreateAsyncScope();

        List<Task> tasks = [];
        var senders = scope.ServiceProvider.GetServices<ISenderBase>();
        tasks.AddRange(senders.Select(sender => sender.Listen(stoppingToken)));
        
        var consumers = scope.ServiceProvider.GetServices<IConsumerBase>();
        tasks.AddRange(consumers.Select(consumer => consumer.Listen(stoppingToken)));

        await Task.WhenAll(tasks);
    }
}