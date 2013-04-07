using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Accounts
{
    public interface IAccountSnapshot : IMemento
    {
        AccountStatus AccountStatus { get; set; }
        Recency Recency { get; set; }
        ClientId ClientId { get; set; }
    }
}