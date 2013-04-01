using System.Collections.Generic;
using Domain.Core.Events;

namespace Domain.Core
{
    public abstract class Aggregate<TIdentity> : Entity<TIdentity>, IAggregate where TIdentity : IHaveIdentity
    {
        private readonly HashSet<IDomainEvent> uncommitedEvents = new HashSet<IDomainEvent>();

        public int Version { get; protected set; }

        protected virtual void RaiseEvent(IDomainEvent domainEvent)
        {
            ApplyEvent(domainEvent);
            uncommitedEvents.Add(domainEvent);
        }

        void IAggregate.ApplyEvent(object @event)
        {
            Version++;
            ApplyEvent(@event);
        }

        protected virtual void ApplyEvent(object @event)
        {
            ((dynamic)this).When((dynamic)@event);
        }

        public void ClearRaisedEvents()
        {
            uncommitedEvents.Clear();
        }

        public IEnumerable<IDomainEvent> GetRaisedEvents()
        {
            return uncommitedEvents;
        }                
    }    
}