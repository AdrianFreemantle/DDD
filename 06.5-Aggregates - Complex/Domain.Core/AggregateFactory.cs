using System.Reflection;

namespace Domain.Core
{
    public static class AggregateFactory
    {
        public static TAggreggate Build<TAggreggate>(IMemento memento) where TAggreggate : IAggregate
        {
            ConstructorInfo constructor = typeof(TAggreggate).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(IHaveIdentity) }, null);

            var aggregate = constructor.Invoke(new object[] { memento.Identity }) as IAggregate;

            if (aggregate != null)
            {
                aggregate.RestoreSnapshot(memento);
            }

            return (TAggreggate)aggregate;
        }
    }
}
