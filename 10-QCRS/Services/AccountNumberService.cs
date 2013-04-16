using System.Linq;
using Domain.Client.Accounts;
using System;
using Domain.Client.Clients;
using Domain.Core.Infrastructure;
using PersistenceModel.Reporting;

namespace Services
{
    public sealed class AccountNumberService : IAccountNumberService
    {
        private readonly IDataQuery dataQuery;

        public AccountNumberService(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public AccountNumber GetNextAccountNumber()
        {
            //normally we would connect to the database to get the next available account number.
            var ticks = DateTime.Now.Ticks.ToString();
            return new AccountNumber(String.Format("A{0}", ticks.Substring(ticks.Length - 6)));
        }

        public AccountNumber GetAccountNumberForClient(ClientId clientId)
        {
            var accountNumber = dataQuery.GetQueryable<ClientView>()
                                         .Where(a => a.IdentityNumber == clientId.Id)
                                         .Select(a => a.AccountNumber)
                                         .FirstOrDefault();

            return String.IsNullOrWhiteSpace(accountNumber)
                ? AccountNumber.Empty()
                : new AccountNumber(accountNumber);
        }

    }
}
