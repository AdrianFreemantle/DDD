using System;
using System.Runtime.Serialization;

using Domain.Client.ValueObjects;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    [DataContract]
    public class ClientRegistered : IDomainEvent
    {
        [DataMember(Order = 1)]
        public ClientId ClientId { get; protected set; }

        [DataMember(Order = 2)]
        public IdentityNumber IdentityNumber { get; protected set; }

        [DataMember(Order = 3)]
        public PersonName ClientName { get; protected set; }

        [DataMember(Order = 4)]
        public TelephoneNumber TelephoneNumber { get; protected set; }

        public ClientRegistered(ClientId clientId, IdentityNumber identityNumber, PersonName personName, TelephoneNumber telephoneNumber)
        {
            ClientId = clientId;
            IdentityNumber = identityNumber;
            ClientName = personName;
            TelephoneNumber = telephoneNumber;
        }
    }
}