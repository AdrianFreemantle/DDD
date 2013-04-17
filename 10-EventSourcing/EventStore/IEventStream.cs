using System;
using System.Collections.Generic;

namespace EventStore
{
    /// <summary>
    /// Indicates the ability to track a series of events and commit them to durable storage.
    /// </summary>
    /// <remarks>
    /// Instances of this class are single threaded and should not be shared between threads.
    /// </remarks>
    public interface IEventStream 
    {
        /// <summary>
        /// Gets the value which uniquely identifies the stream to which the stream belongs.
        /// </summary>
        Guid StreamId { get; }

        /// <summary>
        /// Gets the value which indiciates the most recent committed revision of event stream.
        /// </summary>
        int StreamRevision { get; }

        /// <summary>
        /// Gets the collection of events which have been successfully persisted to durable storage.
        /// </summary>
        IEnumerable<EventMessage> CommittedEvents { get; }

        /// <summary>
        /// Adds the event messages provided to the session to be tracked.
        /// </summary>
        /// <param name="uncommittedEvent">The event to be tracked.</param>
        void Add(EventMessage uncommittedEvent);
    }
}