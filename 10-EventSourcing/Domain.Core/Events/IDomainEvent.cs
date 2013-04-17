using System;

namespace Domain.Core.Events
{
    public interface IDomainEvent
    {
        Guid Source { get; set; }
        int Version { get; set; }
    }

    public abstract class DomainEvent : IDomainEvent
    {
        Guid IDomainEvent.Source { get; set; }
        int IDomainEvent.Version { get; set; }
    }
}