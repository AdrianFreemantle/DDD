using Domain.Client.Accounts;
using Domain.Client.Accounts.Events;
using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;
using Domain.Core.Logging;

namespace PersistenceModel
{   
    public class AccountProjections : IAccountProjections
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof (AccountProjections));
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
            Logger.Verbose(@event.ToString());
        }

        public void When(AccountStatusChanged @event)
        {
            var account = FetchModel(@event.AccountNumber);
            account.AccountStatusId = (int)@event.Status.Status;
            Logger.Verbose(@event.ToString());
        }

        public void When(AccountBilled @event)
        {
            var account = FetchModel(@event.AccountNumber);
            account.Recency = @event.Recency.Value;
            Logger.Verbose(@event.ToString());
        }

        private AccountModel FetchModel(AccountNumber accountNumber)
        {
            return repository.Get<AccountModel>(accountNumber.Id);
        }
    }
}