using System;
using System.Runtime.Serialization;

using Domain.Client.Accounts;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    public class IssuedLoyaltyCard : DomainEvent, IClientEvent
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        [DataMember(Order = 2, IsRequired = true, Name = "CardNumber")]
        public LoyaltyCardNumber CardNumber { get; protected set; }

        public IssuedLoyaltyCard(ClientId clientId, LoyaltyCardNumber loyaltyCardNumber)
        {
            ClientId = clientId;
            CardNumber = loyaltyCardNumber;
        }

        public override string ToString()
        {
            return String.Format("Client {0} was issued loyalty card {1}.", ClientId.Id, CardNumber.Id);
        }
    }
}