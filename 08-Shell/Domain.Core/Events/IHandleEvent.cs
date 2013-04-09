namespace Domain.Core.Events
{
    public interface IHandleEvent<in TEvent> where TEvent : IDomainEvent
    {
        void When(TEvent @event);
    }
}
