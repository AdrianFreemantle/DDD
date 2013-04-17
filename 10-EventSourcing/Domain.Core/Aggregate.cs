using System.Collections.Generic;
using Domain.Core.Events;

namespace Domain.Core
{
    public abstract class Aggregate<TIdentity> : Entity<TIdentity>, IAggregate where TIdentity : IHaveIdentity
    {
        private int version;
        private readonly HashSet<IDomainEvent> changes = new HashSet<IDomainEvent>();

        IEnumerable<IDomainEvent> IAggregate.GetChanges()
        {
            return changes;
        }

        int IAggregate.GetVersion()
        {
            return version;
        }

        protected virtual void RaiseEvent(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                RaiseEvent(@event);
            }
        }

        protected override void RaiseEvent(IDomainEvent @event) 
        {
            ApplyEvent(@event);
            base.RaiseEvent(@event);
            SaveChange(@event);
        }

        protected virtual void SaveChange(IDomainEvent @event)
        {
            version++;
            @event.AggregateId = Identity;
            @event.Version = version;
            changes.Add(@event);
        }               
    }    
}