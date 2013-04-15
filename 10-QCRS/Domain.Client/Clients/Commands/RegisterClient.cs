using System;
using System.Runtime.Serialization;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Clients.Commands
{
    [DataContract]
    public class RegisterClient : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "IdentityNumber")]
        public IdentityNumber IdentityNumber { get; set; }

        [DataMember(Order = 2, IsRequired = true, Name = "ClientName")]
        public PersonName ClientName { get; set; }

        [DataMember(Order = 3, IsRequired = true, Name = "PrimaryContactNumber")]
        public TelephoneNumber PrimaryContactNumber { get; set; }

        protected RegisterClient()
        {
        }

        public RegisterClient(IdentityNumber identityNumber, PersonName clientName, TelephoneNumber primaryContactNumber)
        {            
            Mandate.ParameterNotDefaut(identityNumber, "identityNumber");
            Mandate.ParameterNotDefaut(clientName, "clientName");
            Mandate.ParameterNotDefaut(primaryContactNumber, "primaryContactNumber");

            IdentityNumber = identityNumber;
            ClientName = clientName;
            PrimaryContactNumber = primaryContactNumber;
        }

        public override string ToString()
        {
            return String.Format("Registering client {0} {1} with Id {2}.", ClientName.FirstName, ClientName.Surname, IdentityNumber.Number);
        }
    }
}