using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Accounts
{
    public class AccountSnapshot : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public Recency Recency { get; set; }
    }
}