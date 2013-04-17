using System;
using Domain.Core.Events;

namespace Domain.Core
{
    public interface IEntity 
    {
        Action<IDomainEvent> SaveChangesHandler { get; set; }
        IMemento GetSnapshot();
        void RestoreSnapshot(IMemento memento);
        void ApplyEvent(object @event);    
    }
}