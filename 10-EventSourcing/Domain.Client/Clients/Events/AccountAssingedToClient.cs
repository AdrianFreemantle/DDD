using Domain.Client.Accounts;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    public class AccountAssingedToClient : DomainEvent
    {
        public AccountNumber AccountNumber { get; protected set; }

        public AccountAssingedToClient(AccountNumber accountNumber)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");

            AccountNumber = accountNumber;
        }
    }
}