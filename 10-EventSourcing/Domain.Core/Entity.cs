using System;
using System.Collections.Generic;
using Domain.Core.Events;

namespace Domain.Core
{
    public abstract class Entity<TIdentity> : IEquatable<Entity<TIdentity>>, IEntity where TIdentity : IHaveIdentity
    {
        public TIdentity Identity { get; protected set; }

        Action<IDomainEvent> IEntity.SaveChangesHandler { get; set; }

        void IEntity.ApplyEvent(object @event)
        {
            ApplyEvent(@event);
        }

        protected virtual void ApplyEvent(object @event)
        {
            ((dynamic)this).When((dynamic)@event);
        }

        protected virtual void RaiseEvent(IDomainEvent @event)
        {
            (this as IEntity).SaveChangesHandler(@event);
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TIdentity>);
        }

        public virtual bool Equals(Entity<TIdentity> other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Identity.Equals(Identity);
            }

            return false;
        }

        public static bool operator ==(Entity<TIdentity> left, Entity<TIdentity> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TIdentity> left, Entity<TIdentity> right)
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
            if (memento == null)
            {
                return;
            }

            Identity = (TIdentity)memento.Identity;
            RestoreSnapshot(memento);
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
