using System;

namespace Domain.Core
{
    public interface IEntity
    {
        IMemento GetSnapshot();
        void RestoreSnapshot(IMemento memento);
    }
    
    public abstract class Entity<T> : IEquatable<Entity<T>>, IEntity where T : IHaveIdentity
    {
        public T Identity { get; protected set; }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<T>);
        }

        public virtual bool Equals(Entity<T> other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Identity.Equals(Identity);
            }

            return false;
        }

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !Equals(left, right);
        }

        IMemento IEntity.GetSnapshot()
        {
            var snapshot = GetSnapshot();

            if(snapshot != null)
            {
                snapshot.Identity = Identity;
            }

            return snapshot;
        }

        void IEntity.RestoreSnapshot(IMemento memento)
        {
            if (memento != null)
            {
                RestoreSnapshot(memento);
            }
        }

        protected virtual IMemento GetSnapshot()
        {
            return null;
        }

        protected virtual void RestoreSnapshot(IMemento memento)
        {
            throw new NotImplementedException("The entity does not currently support restoring from a snapshot");
        }
    } 
}
