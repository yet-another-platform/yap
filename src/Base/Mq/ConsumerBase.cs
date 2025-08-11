using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Mq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Mq;

public abstract class ConsumerBase<TRequest, TResponse> : IConsumerBase<TRequest, TResponse>
{
    protected abstract string QueueName { get; }

    private readonly IConnection _connection;
    private readonly ILogger _logger;

    protected ConsumerBase(ILogger logger, IConnection connection)
    {
        if (!connection.IsOpen)
        {
            throw new ArgumentException("MQ connection is not open", nameof(connection));
        }

        _connection = connection;
        _logger = logger;
    }

    public async Task Listen(CancellationToken ct)
    {
        await using var channel = await _connection.CreateChannelAsync(cancellationToken: ct);
        await channel.QueueDeclareAsync(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null,
            cancellationToken: ct);
        await channel.BasicQosAsync(0, 1, false, ct);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var cons = (AsyncEventingBasicConsumer)sender;
            var ch = cons.Channel;
            byte[] response = [];

            byte[] body = eventArgs.Body.ToArray();
            var props = eventArgs.BasicProperties;
            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId
            };

            try
            {
                var request = JsonSerializer.Deserialize<TRequest>(body) ?? throw new UnreachableException();
                var result = await ConsumeAsync(request);
                response = JsonSerializer.SerializeToUtf8Bytes(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while processing request");
            }
            finally
            {
                await ch.BasicPublishAsync(exchange: string.Empty, routingKey: props.ReplyTo!, mandatory: true, basicProperties: replyProps,
                    body: response, ct);
                await ch.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false, cancellationToken: ct);
            }
        };

        await channel.BasicConsumeAsync(QueueName, false, consumer, ct);

        try
        {
            await Task.Delay(Timeout.Infinite, ct);
        }
        catch
        {
            // ignored
        }
    }

    public abstract Task<TResponse> ConsumeAsync(TRequest request);
}