using System;
using System.Runtime.Serialization;
using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Clients.Commands
{
    [DataContract]
    public class SetClientAsDeceased : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        protected SetClientAsDeceased()
        {
        }

        public SetClientAsDeceased(ClientId clientId)
        {
            Mandate.ParameterNotNull(clientId, "accountNumber");

            ClientId = clientId;
        }

        public override string ToString()
        {
            return String.Format("Setting client {0} as deceased", ClientId.Id);
        }
    }
}
