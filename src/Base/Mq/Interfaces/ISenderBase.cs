namespace Mq.Interfaces;

public interface ISenderBase
{
    public string QueueName { get; }
    public Task Listen(CancellationToken cancellationToken);
}