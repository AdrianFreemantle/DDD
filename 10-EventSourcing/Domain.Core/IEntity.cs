using System;
using Domain.Core.Events;

namespace Domain.Core
{
    public interface IEntity 
    {
        IMemento GetSnapshot();
        void RestoreSnapshot(IMemento memento);
        void ApplyEvent(object @event);
        void RegisterChangesHandler(Action<IDomainEvent> handler);
    }
}