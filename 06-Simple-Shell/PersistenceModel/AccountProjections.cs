using System.Linq;
using Domain.Client.Accounts;
using Domain.Client.Events;
using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;

namespace PersistenceModel
{
    public class AccountProjections : IHandleAccountStateTransitions
    {
        private readonly IRepository repository;

        public AccountProjections(IRepository repository)
        {
            this.repository = repository;
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
        }

        public void When(AccountStatusChanged @event)
        {
            var account = FetchModel(@event.AccountNumber);
            account.AccountStatusId = (int)@event.Status.Status;
        }

        public void When(AccountBilled @event)
        {
            var account = FetchModel(@event.AccountNumber);
            account.Recency = @event.Recency.Value;
        }

        private AccountModel FetchModel(AccountNumber accountNumber)
        {
            return repository.GetQueryable<AccountModel>().First(account => account.AccountNumber == accountNumber.Id);
        }
    }
}