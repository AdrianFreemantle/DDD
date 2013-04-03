using System.Runtime.Serialization;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Core.Events;

namespace Domain.Client.Events
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
            ClientId = clientId;
            AccountNumber = accountNumber;
        }
    }
}