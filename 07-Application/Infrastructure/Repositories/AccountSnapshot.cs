using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Infrastructure.Repositories
{
    class AccountSnapshot : IAccountSnapshot
    {
        public IHaveIdentity Identity { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public Recency Recency { get; set; }
        public ClientId ClientId { get; set; }
    }
}