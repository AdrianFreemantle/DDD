using System.Reflection;

namespace Domain.Core
{
    public static class AggregateFactory
    {
        public static TEntity Build<TEntity>(IMemento memento) where TEntity : IAggregate
        {
            ConstructorInfo constructor = typeof(TEntity).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(IHaveIdentity) }, null);

            var aggregate = constructor.Invoke(new object[] { memento.Identity }) as IAggregate;

            if (aggregate != null)
            {
                aggregate.RestoreSnapshot(memento);
            }

            return (TEntity)aggregate;
        }
    }
}
