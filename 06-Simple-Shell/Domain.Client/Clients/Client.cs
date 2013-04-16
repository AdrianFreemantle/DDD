using Domain.Client.Accounts;
using Domain.Client.Events;
using Domain.Client.Services;
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
            Mandate.ParameterNotNull(idNumber, "idNumber");
            Mandate.ParameterNotNull(clientName, "clientName");
            Mandate.ParameterNotNull(primaryContactNumber, "primaryContactNumber");

            var client = new Client();
            client.RaiseEvent(new ClientRegistered(new ClientId(idNumber), idNumber, clientName, primaryContactNumber));
            return client;
        }

        public void CorrectDateOfBirth(DateOfBirth birthDate)
        {
            if (birthDate.GetCurrentAge() < 18)
            {
                throw DomainError.Named("underage", "A client must be older than 18 years.");
            }

            RaiseEvent(new ClientDateOfBirthCorrected(Identity, birthDate));
        }

        public Account OpenAccount(IAccountNumberService accountnumberService)
        {
            Mandate.ParameterNotNull(accountnumberService, "accountnumberService");

            if (this.accountNumber != null)
            {
                throw DomainError.Named("account-exists", "The client already has an account.");
            }

            if (isDeceased)
            {
                throw DomainError.Named("client-deceased", "The client is deceased.");
            }

            this.accountNumber = accountnumberService.GetNextAccountNumber();
            return Account.Open(Identity, accountNumber);
        }

        public void ClientIsDeceased()
        {
            RaiseEvent(new ClientPassedAway(Identity, accountNumber));
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (IClientSnapshot)memento;

            dateOfBirth = snapshot.DateOfBirth;
            clientName = snapshot.ClientName;
            primaryContactNumber = snapshot.PrimaryContactNumber;
            identityNumber = snapshot.IdentityNumber;
            isDeceased = snapshot.IsDeceased;
            accountNumber = snapshot.AccountNumber;
        }
    }
}