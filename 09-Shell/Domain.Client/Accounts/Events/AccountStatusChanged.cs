using System;
using System.Runtime.Serialization;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Accounts.Events
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
            Mandate.ParameterNotNull(accountNumber, "accountNumber");
            Mandate.ParameterNotDefaut(status, "status");

            Status = status;
            AccountNumber = accountNumber;
        }

        public override string ToString()
        {
            return String.Format("Changed status for account {0} to {1}", AccountNumber.Id, Status.Status);
        }
    }
}