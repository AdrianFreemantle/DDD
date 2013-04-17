using System;
using System.Runtime.Serialization;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Accounts.Events
{
    [DataContract]
    public class AccountBilled : DomainEvent, IAccountEvent
    {
        [DataMember(Order = 1, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; protected set; }

        [DataMember(Order = 2, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        [DataMember(Order = 2, IsRequired = true, Name = "Recency")]
        public Recency Recency { get; protected set; }
        
        protected AccountBilled()
        {
        }

        public AccountBilled(AccountNumber accountNumber, ClientId clientId, Recency recency)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");
            Mandate.ParameterNotNullOrEmpty(clientId, "clientId");

            Recency = recency;
            ClientId = clientId;
            AccountNumber = accountNumber;
        }

        public override string ToString()
        {
            return String.Format("Account {0} recency was updated to {1}.", AccountNumber.Id, Recency.Value);
        }
    }
}