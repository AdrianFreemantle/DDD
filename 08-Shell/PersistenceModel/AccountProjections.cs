using System.Linq;
using Domain.Client.Accounts;
using Domain.Client.Events;
using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;
using Domain.Core;

namespace PersistenceModel
{
    public class AccountProjections : IHandleAccountStateTransitions
    {
        private readonly IRepository repository;
        private readonly ILog logger;

        public AccountProjections(IRepository repository, ILog logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public void When(AccountOpened @event)
        {
            var accountModel = new AccountModel
            {
                AccountNumber = @event.AccountNumber.Id,
                ClientId = @event.ClientId.Id,
                AccountStatusId = (int)AccountStatusType.Active,
                Recency = Recency.UpToDate().Value
            };

            repository.Add(accountModel);

            logger.Verbose("Opened account {0} for client {1}", @event.AccountNumber.Id, @event.ClientId.Id);
        }

        public void When(AccountStatusChanged @event)
        {
            var account = FetchModel(@event.AccountNumber);
            account.AccountStatusId = (int)@event.Status.Status;

            logger.Verbose("Changed status for account {0} to {1}", @event.AccountNumber.Id, @event.Status.Status);
        }

        public void When(AccountBilled @event)
        {
            var account = FetchModel(@event.AccountNumber);
            account.Recency = @event.Recency.Value;

            logger.Verbose("Account {0} recency was updated to {1}", @event.AccountNumber.Id, @event.Recency.Value);
        }

        private AccountModel FetchModel(AccountNumber accountNumber)
        {
            return repository.GetQueryable<AccountModel>().First(account => account.AccountNumber == accountNumber.Id);
        }
    }
}