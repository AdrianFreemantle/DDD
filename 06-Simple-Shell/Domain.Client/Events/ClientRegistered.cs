using System.Runtime.Serialization;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Events;

namespace Domain.Client.Events
{
    [DataContract]
    public class ClientRegistered : IDomainEvent
    {
        [DataMember(Order = 0, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        [DataMember(Order = 1, IsRequired = true, Name = "IdentityNumber")]
        public IdentityNumber IdentityNumber { get; protected set; }

        [DataMember(Order = 2, IsRequired = true, Name = "ClientName")]
        public PersonName ClientName { get; protected set; }

        [DataMember(Order = 3, IsRequired = true, Name = "PrimaryContactNumber")]
        public TelephoneNumber PrimaryContactNumber { get; protected set; }

        public ClientRegistered(ClientId clientId, IdentityNumber identityNumber, PersonName clientName, TelephoneNumber primaryContactNumber)
        {
            ClientId = clientId;
            IdentityNumber = identityNumber;
            ClientName = clientName;
            PrimaryContactNumber = primaryContactNumber;
        }
    }
}