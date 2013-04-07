namespace Domain.Core.Infrastructure
{
    public abstract class AggregateRepository<TAggregate> : IAggregateRepository<TAggregate> where TAggregate : class, IAggregate
    {
        public virtual TAggregate Get(object id)
        {
            var aggregate = ActivatorHelper.CreateInstance<TAggregate>();

            if (aggregate != null)
            {
                aggregate.RestoreSnapshot(LoadSnapshot(id));
            }

            return aggregate;
        }

        protected abstract IMemento LoadSnapshot(object id);
    }
}