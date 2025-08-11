namespace Mq.Interfaces;

public interface IConsumerBase
{
    public Task Listen(CancellationToken ct);
}

public interface IConsumerBase<in TRequest, TResponse> : IConsumerBase
{
    public Task<TResponse> ConsumeAsync(TRequest request);
}