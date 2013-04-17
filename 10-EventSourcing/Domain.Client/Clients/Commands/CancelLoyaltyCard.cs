using System;
using System.Runtime.Serialization;

using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Clients.Commands
{
    [DataContract]
    public class CancelLoyaltyCard : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        protected CancelLoyaltyCard()
        {
        }

        public CancelLoyaltyCard(ClientId clientId)
        {
            Mandate.ParameterNotNullOrEmpty(clientId, "clientId");

            ClientId = clientId;
        }

        public override string ToString()
        {
            return String.Format("Client is cancelling {0} loyalty card.", ClientId.Id);
        }
    }
}