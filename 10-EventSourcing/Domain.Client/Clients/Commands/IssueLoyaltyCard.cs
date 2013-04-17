using System;
using System.Runtime.Serialization;

using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Clients.Commands
{
    [DataContract]
    public class IssueLoyaltyCard : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        protected IssueLoyaltyCard()
        {
        }

        public IssueLoyaltyCard(ClientId clientId)
        {
            Mandate.ParameterNotNullOrEmpty(clientId, "clientId");

            ClientId = clientId;
        }

        public override string ToString()
        {
            return String.Format("Issuing loyalty card to client {0}.", ClientId.Id);
        }
    }
}