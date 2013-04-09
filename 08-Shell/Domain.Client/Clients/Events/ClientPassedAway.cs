using System;
using System.Runtime.Serialization;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    [DataContract]
    public class ClientPassedAway : IDomainEvent
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; private set; }

        public ClientPassedAway(ClientId clientId)
        {
            ClientId = clientId;
        }

        public override string ToString()
        {
            return String.Format("Client {0} has passed away. They will be missed.", ClientId.Id);
        }
    }
}