using System;
using System.Collections.Generic;

using Domain.Core.Events;

namespace Domain.Core
{
    public abstract class Entity<T> : IEquatable<Entity<T>>, IPublishEvents, IEntity where T : IHaveIdentity
    {
        private readonly HashSet<IDomainEvent> uncommitedEvents = new HashSet<IDomainEvent>();        
        public T Identity { get; protected set; }

        protected virtual void RaiseEvent(IDomainEvent domainEvent)
        {
            uncommitedEvents.Add(domainEvent);
        }

        public void ClearRaisedEvents()
        {
            uncommitedEvents.Clear();
        }

        public IEnumerable<IDomainEvent> GetRaisedEvents()
        {
            return uncommitedEvents;
        }
        
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
