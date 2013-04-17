using System;

namespace EventStore
{
    public interface IStoreEvents 
    {
        IEventStream OpenStream(Guid streamId);
    }
}
