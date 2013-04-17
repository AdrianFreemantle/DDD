using Domain.Client.Accounts;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    public class AccountAssingedToClient : DomainEvent, IClientEvent
    {
        public ClientId ClientId { get; protected set; }
        public AccountNumber AccountNumber { get; protected set; }

        public AccountAssingedToClient(ClientId clientId, AccountNumber accountNumber)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");
            Mandate.ParameterNotNullOrEmpty(clientId, "clientId");

            ClientId = clientId;
            AccountNumber = accountNumber;
        }

    }
}