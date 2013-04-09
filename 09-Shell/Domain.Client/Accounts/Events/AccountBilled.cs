using System;
using System.Runtime.Serialization;
using Domain.Client.ValueObjects;
using Domain.Core.Events;

namespace Domain.Client.Accounts.Events
{
    [DataContract]
    public class AccountBilled : IDomainEvent
    {
        [DataMember(Order = 0, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; protected set; }

        [DataMember(Order = 1, IsRequired = true, Name = "Recency")]
        public Recency Recency { get; protected set; }

        public AccountBilled(AccountNumber accountNumber, Recency recency)
        {
            Recency = recency;
            AccountNumber = accountNumber;
        }

        public override string ToString()
        {
            return String.Format("Account {0} recency was updated to {1}.", AccountNumber.Id, Recency.Value);
        }
    }
}