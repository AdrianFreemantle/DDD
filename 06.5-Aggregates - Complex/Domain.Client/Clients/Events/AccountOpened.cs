using System.Runtime.Serialization;
using Domain.Client.ValueObjects;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    [DataContract]
    public class AccountOpened : IDomainEvent
    {
        [DataMember(Order = 1)]
        public AccountNumber AccountNumber { get; protected set; }

        public AccountOpened(AccountNumber accountNumber)
        {
            AccountNumber = accountNumber;
        }
    }
}