using System.Runtime.Serialization;
using Domain.Client.Clients;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Accounts.Events
{
    [DataContract]
    public class AccountOpened : IDomainEvent
    {
        [DataMember(Order = 0, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        [DataMember(Order = 1, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; protected set; }

        public AccountOpened(ClientId clientId, AccountNumber accountNumber)
        {
            Mandate.ParameterNotNull(clientId, "clientId");
            Mandate.ParameterNotNull(accountNumber, "accountNumber");

            ClientId = clientId;
            AccountNumber = accountNumber;
        }

        public override string ToString()
        {
            return string.Format("Opened account {0} for client {1}", AccountNumber.Id, ClientId.Id);
        }
    }
}