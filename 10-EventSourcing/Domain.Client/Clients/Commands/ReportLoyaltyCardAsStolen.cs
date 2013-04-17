using System;
using System.Runtime.Serialization;

using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Clients.Commands
{
    [DataContract]
    public class ReportLoyaltyCardAsStolen : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        protected ReportLoyaltyCardAsStolen()
        {
        }

        public ReportLoyaltyCardAsStolen(ClientId clientId)
        {
            Mandate.ParameterNotNullOrEmpty(clientId, "clientId");

            ClientId = clientId;
        }

        public override string ToString()
        {
            return String.Format("Reporting client {0} loyalty card as stolen.", ClientId.Id);
        }
    }
}