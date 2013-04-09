namespace Domain.Core.Infrastructure
{
    public abstract class AggregateRepository<TAggregate> : IAggregateRepository<TAggregate> where TAggregate : class, IAggregate
    {
        protected abstract IMemento LoadSnapshot(object id);

        public virtual TAggregate Get<TKey>(IdentityBase<TKey> id)
        {
            return Get(id.Id);
        }

        public virtual TAggregate Get(object id)
        {
            var aggregate = ActivatorHelper.CreateInstance<TAggregate>();

            if (aggregate != null)
            {
                aggregate.RestoreSnapshot(LoadSnapshot(id));
            }

            return aggregate;
        }               
    }
}