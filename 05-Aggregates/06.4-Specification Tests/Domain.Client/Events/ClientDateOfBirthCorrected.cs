using System.Runtime.Serialization;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Events;

namespace Domain.Client.Events
{
    [DataContract]
    public class ClientDateOfBirthCorrected : IDomainEvent
    {
        [DataMember(Order = 0, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        [DataMember(Order = 1, IsRequired = true, Name = "DateOfBirth")]
        public DateOfBirth DateOfBirth { get; protected set; }

        public ClientDateOfBirthCorrected(ClientId clientId, DateOfBirth dateOfBirth)
        {
            ClientId = clientId;
            DateOfBirth = dateOfBirth;
        }
    }
}