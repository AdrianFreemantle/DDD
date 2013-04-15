using System;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Infrastructure;

namespace PersistenceModel.Repositories
{   
    public sealed class AccountRepository : IAccountRepository 
    {
        private readonly IRepository repository;

        public AccountRepository(IRepository repository)
        {
            this.repository = repository;
        }

        public Account Get<TKey>(IdentityBase<TKey> id)
        {
            var accountModel = repository.Get<AccountModel>(id.Id);
            return BuildAccount(accountModel);
        }
        
        private Account BuildAccount(AccountModel accountModel)
        {
            var account = ActivatorHelper.CreateInstance<Account>();
            (account as IAggregate).RestoreSnapshot(LoadSnapshot(accountModel));
            return account;
        }

        private IMemento LoadSnapshot(AccountModel accountModel)
        {
            return new AccountSnapshot
            {
                Identity = new AccountNumber(accountModel.AccountNumber),
                AccountStatus = new AccountStatus((AccountStatusType)accountModel.AccountStatusId),
                Recency = new Recency(accountModel.Recency),
                ClientId = new ClientId(accountModel.ClientId)                
            };
        }

        class AccountSnapshot : IAccountSnapshot
        {
            public IHaveIdentity Identity { get; set; }
            public AccountStatus AccountStatus { get; set; }
            public Recency Recency { get; set; }
            public ClientId ClientId { get; set; }
        }
    }
}