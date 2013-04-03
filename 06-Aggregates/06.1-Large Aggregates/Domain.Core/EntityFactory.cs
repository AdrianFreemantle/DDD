using System.Reflection;

namespace Domain.Core
{
    public static class EntityFactory
    {
        public static TEntity Build<TEntity>(IMemento memento) where TEntity : IEntity
        {
            ConstructorInfo constructor = typeof(TEntity).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(IHaveIdentity) }, null);

            var entity = constructor.Invoke(new object[] { memento.Identity }) as IEntity;

            if (entity != null)
            {
                entity.RestoreSnapshot(memento);
            }

            return (TEntity)entity;
        }
    }
}
