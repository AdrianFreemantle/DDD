using System;
using System.Runtime.Serialization;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    [DataContract]
    public class ClientDateOfBirthCorrected : DomainEvent, IClientEvent
    {
        [DataMember(Order = 0, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        [DataMember(Order = 1, IsRequired = true, Name = "DateOfBirth")]
        public DateOfBirth DateOfBirth { get; protected set; }

        public ClientDateOfBirthCorrected(ClientId clientId, DateOfBirth dateOfBirth)
        {
            Mandate.ParameterNotNull(clientId, "clientId");
            Mandate.ParameterNotDefaut(dateOfBirth, "dateOfBirth");

            ClientId = clientId;
            DateOfBirth = dateOfBirth;
        }

        public override string ToString()
        {
            return String.Format("Corrected date of birth for client {0} to {1}.", ClientId.Id, DateOfBirth.Date.ToShortDateString());
        }
    }
}