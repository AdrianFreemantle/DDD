namespace Domain.Core.Infrastructure
{
    public abstract class AggregateRepository<TAggregate> : IAggregateRepository<TAggregate> where TAggregate : class, IAggregate
    {
        public virtual TAggregate Get(object id)
        {
            return AggregateFactory.Build<TAggregate>(LoadSnapshot(id));
        }

        protected abstract IMemento LoadSnapshot(object id);
    }
}