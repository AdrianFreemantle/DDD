namespace Domain.Core
{
    public interface IAggregate : IEntity
    {
        void ApplyEvent(object @event);
    }
}