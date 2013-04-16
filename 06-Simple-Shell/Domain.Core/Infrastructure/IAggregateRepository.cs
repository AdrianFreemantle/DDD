namespace Domain.Core.Infrastructure
{
    public interface IAggregateRepository<out TAggregate> where TAggregate : class, IAggregate
    {
        TAggregate Get(object id);
    }
}