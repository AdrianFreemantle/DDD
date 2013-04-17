using System;
using System.Runtime.Serialization;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    [DataContract]
    public class ClientRegistered : DomainEvent, IClientEvent
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
            Mandate.ParameterNotNull(clientId, "clientId");
            Mandate.ParameterNotDefaut(identityNumber, "identityNumber");
            Mandate.ParameterNotDefaut(clientName, "clientName");
            Mandate.ParameterNotDefaut(primaryContactNumber, "primaryContactNumber");

            ClientId = clientId;
            IdentityNumber = identityNumber;
            ClientName = clientName;
            PrimaryContactNumber = primaryContactNumber;
        }

        public override string ToString()
        {
            return String.Format("Registered new client {0} {1} with Id {2}.", ClientName.FirstName, ClientName.Surname, IdentityNumber.Number);
        }
    }
}