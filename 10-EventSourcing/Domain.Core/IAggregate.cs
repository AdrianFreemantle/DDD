using System.Collections.Generic;
using Domain.Core.Events;

namespace Domain.Core
{
    public interface IAggregate : IEntity
    {
        int GetVersion();
        IEnumerable<IDomainEvent> GetChanges();
    }
}