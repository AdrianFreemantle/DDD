namespace Domain.Core.Infrastructure
{
    public static class AggregateFactory
    {
        public static TAggreggate Build<TAggreggate>(IMemento memento) where TAggreggate : class, IAggregate
        {
            var aggregate = ActivatorHelper.CreateInstance<TAggreggate>();

            if (aggregate != null)
            {
                aggregate.RestoreSnapshot(memento);
            }

            return aggregate;
        }
    }
}
