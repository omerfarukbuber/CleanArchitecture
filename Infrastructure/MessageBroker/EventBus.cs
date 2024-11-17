using Application.EventBus;
using MassTransit;

namespace Infrastructure.MessageBroker;

public sealed class EventBus(IPublishEndpoint publishEndpoint) : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public Task PublishEvent<T>(T message,
        CancellationToken cancellationToken = default) where T : class =>
        _publishEndpoint.Publish(message,
            cancellationToken);
}