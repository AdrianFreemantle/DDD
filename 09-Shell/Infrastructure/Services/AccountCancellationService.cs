using System.Linq;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.Clients.Services;
using Domain.Core.Infrastructure;
using PersistenceModel;

namespace Infrastructure.Services
{
    public sealed class AccountCancellationService : IAccountCancellationService
    {
        private readonly IDataQuery dataQuery;
        private readonly IAggregateRepository<Account> accountRepository;

        public AccountCancellationService(IDataQuery dataQuery, IAggregateRepository<Account> accountRepository)
        {
            this.dataQuery = dataQuery;
            this.accountRepository = accountRepository;
        }

        public void CancelClientAccount(ClientId clientId)
        {
            var accountNumber = dataQuery.GetQueryable<AccountModel>()
                                         .Where(a => a.ClientId == clientId.Id)
                                         .Select(a => a.AccountNumber)
                                         .FirstOrDefault();

            if (accountNumber == null)
            {
                return;
            }

            var account = accountRepository.Get(accountNumber);
            account.Cancel();
        }
    }
}