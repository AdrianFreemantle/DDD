using System.Collections.Generic;
using Domain.Core.Events;

namespace Domain.Core
{
    public interface IEntity 
    {
        IMemento GetSnapshot();
        void RestoreSnapshot(IMemento memento);
        IEnumerable<IDomainEvent> GetRaisedEvents();
        void ClearEvents();
    }
}