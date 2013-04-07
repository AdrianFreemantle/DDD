using System.Linq;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Infrastructure;
using PersistenceModel;

namespace Infrastructure.Repositories
{
    public class AccountRepository : AggregateRepository<Account>
    {
        private readonly IRepository repository;

        public AccountRepository(IRepository repository)
        {
            this.repository = repository;
        }

        protected override IMemento LoadSnapshot(object id)
        {
            var accountModel = repository.GetQueryable<AccountModel>()
                                        .First(client => client.AccountNumber == id.ToString());

            return new AccountSnapshot
            {
                Identity = new AccountNumber(accountModel.AccountNumber),
                AccountStatus = new AccountStatus((AccountStatusType)accountModel.AccountStatusId),
                Recency = new Recency(accountModel.Recency),
                ClientId = new ClientId(accountModel.ClientId)                
            };
        }
    }
}