using System.Runtime.Serialization;

using Domain.Client.ValueObjects;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    [DataContract]
    public class ClientDateOfBirthCorrected : IDomainEvent
    {
        [DataMember(Order = 1)]
        public string ClientId { get; set; }

        [DataMember(Order = 2)]
        public DateOfBirth DateOfBirth { get; set; }

        public ClientDateOfBirthCorrected(string clientId, DateOfBirth dateOfBirth)
        {
            ClientId = clientId;
            DateOfBirth = dateOfBirth;
        }

        public override string ToString()
        {
            return "Client date of birth corrected.";
        }
    }
}