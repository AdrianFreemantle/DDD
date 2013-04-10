namespace Domain.Core.Events
{
    public interface IPublishEvents
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IDomainEvent;
    }
}