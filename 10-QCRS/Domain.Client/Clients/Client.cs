using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{
    public partial class Client : Aggregate<ClientId>
    {       
        protected Client()
        {
        }

        public static Client RegisterClient(IdentityNumber idNumber, PersonName clientName, TelephoneNumber primaryContactNumber)
        {
            Mandate.ParameterNotDefaut(idNumber, "idNumber");
            Mandate.ParameterNotDefaut(clientName, "clientName");
            Mandate.ParameterNotDefaut(primaryContactNumber, "primaryContactNumber");

            var client = new Client();
            client.RaiseEvent(new ClientRegistered(new ClientId(idNumber), idNumber, clientName, primaryContactNumber));
            return client;
        }

        public void CorrectDateOfBirth(DateOfBirth birthDate)
        {
            Mandate.ParameterNotDefaut(birthDate, "birthDate");

            if (birthDate.GetCurrentAge() < 18)
            {
                throw DomainError.Named("underage", "A client must be older than 18 years.");
            }

            RaiseEvent(new ClientDateOfBirthCorrected(Identity, birthDate));
        }

        public bool ClientMayOpenAccount()
        {
            var clientAge = dateOfBirth.GetCurrentAge();
            return (!isDeceased) && (clientAge >= 18);
        }

        public void ClientIsDeceased()
        {
            RaiseEvent(new ClientPassedAway(Identity));
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (IClientSnapshot)memento;

            dateOfBirth = snapshot.DateOfBirth;
            clientName = snapshot.ClientName;
            primaryContactNumber = snapshot.PrimaryContactNumber;
            identityNumber = snapshot.IdentityNumber;
            isDeceased = snapshot.IsDeceased;
        }
    }
}