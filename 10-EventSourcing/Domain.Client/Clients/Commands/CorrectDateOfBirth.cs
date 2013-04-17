using System;
using System.Runtime.Serialization;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Clients.Commands
{
    [DataContract]
    public class CorrectDateOfBirth : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ClientId")]
        public ClientId ClientId { get; protected set; }

        [DataMember(Order = 2, IsRequired = true, Name = "DateOfBirth")]
        public DateOfBirth DateOfBirth { get; protected set; }

        protected CorrectDateOfBirth()
        {
        }

        public CorrectDateOfBirth(ClientId clientId, DateOfBirth dateOfBirth)
        {
            Mandate.ParameterNotNullOrEmpty(clientId, "clientId");
            Mandate.ParameterNotDefaut(dateOfBirth, "dateOfBirth");

            ClientId = clientId;
            DateOfBirth = dateOfBirth;
        }

        public override string ToString()
        {
            return String.Format("Correcting client {0} date of bith to {1}.", ClientId.Id, DateOfBirth.Date.ToShortDateString());
        }
    }
}