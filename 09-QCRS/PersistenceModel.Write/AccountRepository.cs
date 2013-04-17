using Domain.Client.Accounts;
using Domain.Core;
using Domain.Core.Infrastructure;

namespace PersistenceModel.Write
{   
    public sealed class AccountRepository : IAccountRepository 
    {
        private readonly IDocumentStore documentStore;

        public AccountRepository(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public Account Get(IHaveIdentity id)
        {
            var snapshot = documentStore.Get<AccountSnapshot>(id.GetSurrogateId());
            var account = ActivatorHelper.CreateInstance<Account>();
            ((IAggregate)account).RestoreSnapshot(snapshot);
            return account;
        }

        public void Save(Account account)
        {
            IMemento memento = ((IAggregate)account).GetSnapshot();
            documentStore.Save(memento.Identity.GetSurrogateId(), memento);
        }
    }
}