using System;
using System.Runtime.Serialization;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Accounts.Events
{
    [DataContract]
    public class AccountBilled : IDomainEvent
    {
        [DataMember(Order = 1, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; protected set; }

        [DataMember(Order = 2, IsRequired = true, Name = "Recency")]
        public Recency Recency { get; protected set; }
        
        protected AccountBilled()
        {
        }

        public AccountBilled(AccountNumber accountNumber, Recency recency)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");
            Mandate.ParameterNotDefaut(recency, "recency");

            Recency = recency;
            AccountNumber = accountNumber;
        }

        public override string ToString()
        {
            return String.Format("Account {0} recency was updated to {1}.", AccountNumber.Id, Recency.Value);
        }
    }
}