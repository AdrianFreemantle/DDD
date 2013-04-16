using System.Runtime.Serialization;
using Domain.Client.Clients;
using Domain.Core.Events;

namespace Domain.Client.Events
{
    [DataContract]
    public class ClientPassedAway : IDomainEvent
    {
        [DataMember(Order = 0, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; private set; }

        public ClientPassedAway(ClientId clientId)
        {
            ClientId = clientId;
        }
    }
}