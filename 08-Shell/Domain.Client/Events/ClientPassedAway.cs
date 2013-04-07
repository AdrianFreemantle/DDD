using System.Runtime.Serialization;
using Domain.Client.Clients;
using Domain.Core.Events;
using Domain.Client.Accounts;

namespace Domain.Client.Events
{
    [DataContract]
    public class ClientPassedAway : IDomainEvent
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; private set; }

        [DataMember(Order = 2, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; private set; }

        public ClientPassedAway(ClientId clientId, AccountNumber accountNumber)
        {
            ClientId = clientId;
            AccountNumber = accountNumber;
        }
    }
}