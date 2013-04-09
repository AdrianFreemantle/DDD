namespace Domain.Core.Infrastructure
{
    public interface IAggregateRepository<out TAggregate> where TAggregate : class, IAggregate
    {
        TAggregate Get(object id);
        TAggregate Get<TKey>(IdentityBase<TKey> id);
    }
}