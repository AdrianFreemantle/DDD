using System;
using System.Runtime.Serialization;
using Domain.Client.Clients;
using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Accounts.Commands
{
    [DataContract]
    public class OpenAccount : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "AccountNumber")]
        public ClientId ClientId { get; protected set; }

        protected OpenAccount()
        {
        }

        public OpenAccount(ClientId clientId)
        {
            Mandate.ParameterNotNullOrEmpty(clientId, "clientId");

            ClientId = clientId;
        }

        public override string ToString()
        {
            return String.Format("Opening account for client {0}.", ClientId.Id);
        }
    }
}