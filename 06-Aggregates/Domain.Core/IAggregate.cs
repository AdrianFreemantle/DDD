namespace Domain.Core
{
    public interface IAggregate : IEntity, IPublishEvents
    {
        int Version { get; }
        void ApplyEvent(object @event);
    }
}