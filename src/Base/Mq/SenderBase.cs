using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Mq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Types.Types.Option;

namespace Mq;

public abstract class SenderBase<TRequest, TResult> : ISenderBase
{
    public abstract string QueueName { get; }

    private readonly IConnection _connection;
    private readonly ILogger _logger;
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<Option<TResult>>> _callbackMapper = [];

    private IChannel? _channel;
    private string _replyQueueName = string.Empty;

    protected SenderBase(ILogger logger, IConnection connection)
    {
        if (!connection.IsOpen)
        {
            throw new ArgumentException("MQ connection is not open", nameof(connection));
        }

        _connection = connection;
        _logger = logger;
    }

    public async Task Listen(CancellationToken cancellationToken)
    {
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        var queueDeclareResult = await _channel.QueueDeclareAsync(cancellationToken: cancellationToken);
        _replyQueueName = queueDeclareResult.QueueName;
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            if (!Guid.TryParse(ea.BasicProperties.CorrelationId, out var correlationId) ||
                !_callbackMapper.TryRemove(correlationId, out var taskCompletionSource))
            {
                return Task.CompletedTask;
            }

            byte[] body = ea.Body.ToArray();
            try
            {
                var response = JsonSerializer.Deserialize<TResult>(body) ?? throw new UnreachableException();
                taskCompletionSource.TrySetResult(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deserializing response");
                taskCompletionSource.TrySetResult(new Error { Message = "Failed to deserialize response" });
            }

            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(_replyQueueName, true, consumer, cancellationToken: cancellationToken);
    }

    protected async Task<Option<TResult>> SendInternalAsync(TRequest request, Guid correlationId, CancellationToken cancellationToken)
    {
        if (_channel == null)
        {
            throw new InvalidOperationException("MQ channel is null");
        }

        var props = new BasicProperties
        {
            CorrelationId = correlationId.ToString(),
            ReplyTo = _replyQueueName
        };

        var taskCompletionSource = new TaskCompletionSource<Option<TResult>>(TaskCreationOptions.RunContinuationsAsynchronously);
        _callbackMapper.TryAdd(correlationId, taskCompletionSource);

        byte[] messageBytes = JsonSerializer.SerializeToUtf8Bytes(request);
        await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: QueueName, mandatory: true, basicProperties: props, body: messageBytes,
            cancellationToken: cancellationToken);
        await using var ctr = cancellationToken.Register(() =>
        {
            _callbackMapper.TryRemove(correlationId, out _);
            taskCompletionSource.SetCanceled(cancellationToken);
        });

        return await taskCompletionSource.Task;
    }
}