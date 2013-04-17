using System.Collections.Generic;
using Domain.Core.Events;

namespace Domain.Core
{
    public interface IAggregate : IEntity
    {
        int GetVersion();
        void LoadFromHistory(IEnumerable<IDomainEvent> domainEvents);
        IEnumerable<IDomainEvent> GetChanges();
    }
}