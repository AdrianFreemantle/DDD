namespace Domain.Core.Events
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IDomainEvent;
    }
}