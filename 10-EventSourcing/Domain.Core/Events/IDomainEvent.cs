namespace Domain.Core.Events
{
    public interface IDomainEvent
    {
        IHaveIdentity AggregateId { get; set; }
        int Version { get; set; }
    }

    public abstract class DomainEvent : IDomainEvent
    {
        public IHaveIdentity AggregateId { get; set; }
        public int Version { get; set; }
    }
}