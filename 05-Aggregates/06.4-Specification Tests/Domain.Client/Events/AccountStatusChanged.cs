using System.Runtime.Serialization;
using Domain.Client.Accounts;
using Domain.Client.ValueObjects;
using Domain.Core.Events;

namespace Domain.Client.Events
{
    [DataContract]
    public class AccountStatusChanged : IDomainEvent
    {       
        [DataMember(Order = 0, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; protected set; }

        [DataMember(Order = 1, IsRequired = true, Name = "Status")]
        public AccountStatus Status { get; protected set; }

        public AccountStatusChanged(AccountNumber accountNumber, AccountStatus status)
        {
            Status = status;
            AccountNumber = accountNumber;
        }
    }
}