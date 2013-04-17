using System;
using System.Collections.Generic;

namespace EventStore
{
    public class InMemoryEventStream : IEventStream
    {
        public Guid StreamId { get; private set; }
        public int StreamRevision { get; private set; }
        public IEnumerable<EventMessage> CommittedEvents { get { return committedEvents; }}

        private readonly List<EventMessage> committedEvents; 

        public InMemoryEventStream(Guid streamId)
        {
            StreamId = streamId;
            StreamRevision = 0;
            committedEvents = new List<EventMessage>();
        }
        
        public void Add(EventMessage uncommittedEvent)
        {
            committedEvents.Add(uncommittedEvent);
            StreamRevision++;
        }
    }
}