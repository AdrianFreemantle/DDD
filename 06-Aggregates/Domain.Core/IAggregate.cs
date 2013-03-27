namespace Domain.Core
{
    public interface IAggregate : IEntity, IPublishEvents
    {
        void ApplyEvent(object @event);
    }
}