using System.Runtime.Serialization;

using Domain.Core.Events;

namespace Tests.Clients
{
    [DataContract]
    public class ClientRegistered : IDomainEvent
    {
        [DataMember(Order = 1)]
        public string ClientId { get; protected set; }

        [DataMember(Order = 2)]
        public string FirstName { get; protected set; }

        [DataMember(Order = 3)]
        public string Surname { get; protected set; }

        [DataMember(Order = 4)]
        public string TelephoneNumber { get; protected set; }

        public ClientRegistered(string clientId, string firstName, string surname, string telephoneNumber)
        {
            ClientId = clientId;
            FirstName = firstName;
            Surname = surname;
            TelephoneNumber = telephoneNumber;
        }
    }
}