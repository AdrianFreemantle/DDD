using System;
using Domain.Client.Accounts.Events;
using Domain.Core.Infrastructure;

namespace PersistenceModel.Reporting.Projections
{
    public class AccountStatusHistoryProjection
    {
        private readonly IRepository repository;

        public AccountStatusHistoryProjection(IRepository repository)
        {
            this.repository = repository;
        }

        public void When(AccountStatusChanged @event)
        {
            var accountStatusLookup = repository.Get<AccountStatusLookup>((int)@event.Status.Status);
            
            var accountStatusHistoryView = new AccountStatusHistoryView
            {
                AccountNumber = @event.AccountNumber.Id,
                AccountStatus = accountStatusLookup,
                ChangedDate = DateTime.Now
            };

            repository.Add(accountStatusHistoryView);
        }
    }
}