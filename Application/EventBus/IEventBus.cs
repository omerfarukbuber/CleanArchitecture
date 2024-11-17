namespace Application.EventBus;

public interface IEventBus
{
    Task PublishEvent<T>(T message, CancellationToken cancellationToken = default) where T : class;
}