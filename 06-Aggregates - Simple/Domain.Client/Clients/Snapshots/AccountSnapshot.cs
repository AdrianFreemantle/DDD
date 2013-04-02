using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients.Snapshots
{
    public class AccountSnapshot : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public Recency Recency { get; set; }
        public BillingDate BillingDate { get; set; }
    }
}