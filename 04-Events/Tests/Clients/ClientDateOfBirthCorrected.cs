using System;
using System.Runtime.Serialization;

using Domain.Core.Events;

namespace Tests.Clients
{
    [DataContract]
    public class ClientDateOfBirthCorrected : IDomainEvent
    {
        [DataMember(Order = 1)]
        public string ClientId { get; set; }

        [DataMember(Order = 2)]
        public DateTime DateOfBirth { get; set; }

        public ClientDateOfBirthCorrected(string clientId, DateTime dateOfBirth)
        {
            ClientId = clientId;
            DateOfBirth = dateOfBirth;
        }
    }
}