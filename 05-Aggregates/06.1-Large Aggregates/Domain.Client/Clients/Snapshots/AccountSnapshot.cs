using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients.Snapshots
{
    public class AccountSnapshot : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public BillingResult[] BillingResults { get; set; }
    }
}