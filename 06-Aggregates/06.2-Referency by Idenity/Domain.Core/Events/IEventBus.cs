namespace Domain.Core.Events
{
    public interface IEventBus
    {
        void Submit<TEvent>(TEvent @event) where TEvent : IDomainEvent;
    }
}