using System;
using System.Collections.Generic;

namespace EventStore
{
    public class InMemoryEventStore : IStoreEvents
    {
        private readonly Dictionary<Guid, IEventStream> streams = new Dictionary<Guid, IEventStream>();

        public IEventStream CreateStream(Guid streamId)
        {
            if (streams.ContainsKey(streamId))
            {
                throw new InvalidOperationException("The stream already exists");
            }

            streams.Add(streamId, new InMemoryEventStream(streamId));
            return streams[streamId];
        }

        public IEventStream OpenStream(Guid streamId)
        {
            if (!streams.ContainsKey(streamId))
            {
                streams.Add(streamId, new InMemoryEventStream(streamId));
            }

            return streams[streamId];
        }
    }
}