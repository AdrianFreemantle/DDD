using System.Collections.Generic;

using Domain.Core.Events;

namespace Tests
{
    class EventHandlerHelper
    {
        public List<IDomainEvent> RaisedEvents { get; private set; }

        public EventHandlerHelper()
        {
            RaisedEvents = new List<IDomainEvent>();
        }

        public void Handle(IDomainEvent @event)
        {
            RaisedEvents.Add(@event);
        }
    }
}