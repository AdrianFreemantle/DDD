using System.Collections.Generic;
using Domain.Core.Events;

namespace Tests.TestHelpers
{
    class EventBusStub : IEventBus
    {
        readonly EventHandlerStub eventHandler = new EventHandlerStub();

        public IEnumerable<IDomainEvent> RaisedEvents { get { return eventHandler.RaisedEvents; } } 

        public void Submit<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            eventHandler.Handle(@event);
        }
    }
}