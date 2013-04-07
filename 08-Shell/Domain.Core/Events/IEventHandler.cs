namespace Domain.Core.Events
{
    public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        void When(TEvent @event);
    }
}
