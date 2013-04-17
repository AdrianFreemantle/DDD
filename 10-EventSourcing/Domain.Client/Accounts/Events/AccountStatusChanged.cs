using System;
using System.Runtime.Serialization;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Accounts.Events
{
    public interface IAccountEvent : IDomainEvent
    {
        AccountNumber AccountNumber { get; }
    }

    [DataContract]
    public class AccountStatusChanged : DomainEvent, IAccountEvent
    {
        [DataMember(Order = 1, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; protected set; }

        [DataMember(Order = 2, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        [DataMember(Order = 3, IsRequired = true, Name = "Status")]
        public AccountStatus Status { get; protected set; }

        public AccountStatusChanged(AccountNumber accountNumber, ClientId clientId, AccountStatus status)
        {
            Mandate.ParameterNotNull(accountNumber, "accountNumber");
            Mandate.ParameterNotNull(clientId, "clientId");
            Mandate.ParameterNotDefaut(status, "status");

            Status = status;
            AccountNumber = accountNumber;
            ClientId = clientId;
        }

        public override string ToString()
        {
            return String.Format("Changed status for account {0} to {1}", AccountNumber.Id, Status.Status);
        }
    }
}