using System.Collections.Generic;
using Domain.Core.Events;

namespace Domain.Core
{
    public abstract class Aggregate<TIdentity> : Entity<TIdentity>, IAggregate where TIdentity : IHaveIdentity
    {
        void IAggregate.ApplyEvent(object @event)
        {
            ApplyEvent(@event);
        }

        protected override void RaiseEvent<TEvent>(TEvent @event) 
        {
            ApplyEvent(@event);
            base.RaiseEvent(@event);
        }

        protected virtual void RaiseEvent(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                RaiseEvent(@event);
            }
        }

        protected virtual void ApplyEvent(object @event)
        {
            ((dynamic)this).When((dynamic)@event);
        }
    }    
}