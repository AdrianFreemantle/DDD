using Domain.Client.Accounts;
using Domain.Core;
using Domain.Core.Infrastructure;
using EventStore;

namespace PersistenceModel.Write
{   
    public sealed class AccountRepository : IAccountRepository 
    {
        private readonly IStoreEvents eventStore;

        public AccountRepository(IStoreEvents eventStore)
        {
            this.eventStore = eventStore;
        }

        public Account Get(IHaveIdentity id)
        {
            
        }

        public void Save(Account account)
        {
           
        }
    }
}