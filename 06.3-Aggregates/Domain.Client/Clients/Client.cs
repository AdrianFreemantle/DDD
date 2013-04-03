using Domain.Client.Accounts;
using Domain.Client.Events;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Clients
{   
    public class Client : Aggregate<ClientId>
    {
        private IdentityNumber identityNumber;
        private PersonName clientName;
        private TelephoneNumber primaryContactNumber;
        private DateOfBirth dateOfBirth;
        private bool isDeceased;

        private AccountNumber accountNumber;

        protected Client()
        {
        }

        public static Client RegisterClient(IdentityNumber idNumber, PersonName clientName, TelephoneNumber primaryContactNumber)
        {
            var client = new Client
            {
                Identity = new ClientId(idNumber),
                identityNumber = idNumber,
                clientName = clientName,
                primaryContactNumber = primaryContactNumber,
                dateOfBirth = idNumber.GetDateOfBirth(),
            };

            DomainEvent.Current.Raise(new ClientRegistered(client.Identity, client.identityNumber, client.clientName, client.primaryContactNumber));
            return client;
        }

        public Account OpenAccount(AccountNumber accountNumber)
        {
            Mandate.ParameterNotNull(accountNumber, "accountNumber");

            if (this.accountNumber != null)
            {
                throw DomainError.Named("account-exists", "The client already has an account.");
            }

            if (isDeceased)
            {
                throw DomainError.Named("client-deceased", "The client is deceased.");
            }

            this.accountNumber = accountNumber;
            return Account.Open(Identity, accountNumber);
        }

        public void CorrectDateOfBirth(DateOfBirth birthDate)
        {
            if (birthDate.GetCurrentAge() < 18)
            {
                throw DomainError.Named("underage", "A client must be older than 18 years.");
            }

            dateOfBirth = birthDate;
            DomainEvent.Current.Raise(new ClientDateOfBirthCorrected(Identity, dateOfBirth));
        }

        public void ClientIsDeceased()
        {
            isDeceased = true;
            DomainEvent.Current.Raise(new ClientPassedAway(Identity));
        }

        protected override IMemento GetSnapshot()
        {
            return new ClientSnapshot
            {
                DateOfBirth = dateOfBirth,
                ClientName = clientName,
                PrimaryContactNumber = primaryContactNumber,
                IdentityNumber = identityNumber
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (ClientSnapshot)memento;

            dateOfBirth = snapshot.DateOfBirth;
            clientName = snapshot.ClientName;
            primaryContactNumber = snapshot.PrimaryContactNumber;
            identityNumber = snapshot.IdentityNumber;
        }
    }
}