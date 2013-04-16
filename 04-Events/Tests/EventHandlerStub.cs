using System.Collections.Generic;

using Domain.Core.Events;

namespace Tests
{
    class EventHandlerStub 
    {
        public List<IDomainEvent> RaisedEvents { get; private set; }

        public EventHandlerStub()
        {
            RaisedEvents = new List<IDomainEvent>();
        }

        public void Handle(IDomainEvent @event)
        {
            RaisedEvents.Add(@event);
        }
    }
}