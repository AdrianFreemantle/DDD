using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients.Snapshots
{
    public interface IAccountSnapshot : IMemento
    {
        Recency Recency { get; }
    }
}