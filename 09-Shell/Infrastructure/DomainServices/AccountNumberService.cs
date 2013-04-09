using Domain.Client.Accounts;
using System;
using Domain.Client.Clients.Services;

namespace Infrastructure.DomainServices
{
    public sealed class AccountNumberService : IAccountNumberService
    {
        public AccountNumber GetNextAccountNumber()
        {
            //normally we would connect to the database to get the next available account number.
            var ticks = DateTime.Now.Ticks.ToString();
            return new AccountNumber(String.Format("A{0}", ticks.Substring(ticks.Length - 6)));
        }
    }
}
