namespace Domain.Core.Infrastructure
{
    public interface IAggregateRepository<TAggregate> where TAggregate : class, IAggregate
    {
        TAggregate Get(IHaveIdentity identity);
        void Save(TAggregate aggregate);
    }
}