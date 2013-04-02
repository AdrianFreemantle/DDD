using Domain.Client.Clients.Snapshots;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{   
    public class Client : Aggregate<ClientId>
    {
        private IdentityNumber identityNumber;
        private PersonName clientName;
        private TelephoneNumber primaryContactNumber;
        private DateOfBirth dateOfBirth;

        private Account account;

        protected Client()
        {
        }

        public static Client RegisterClient(IdentityNumber idNumber, PersonName clientName, TelephoneNumber telephoneNumber)
        {
            return new Client
            {
                Identity = new ClientId(idNumber),
                identityNumber = idNumber,
                clientName = clientName,
                primaryContactNumber = telephoneNumber,
                dateOfBirth = idNumber.GetDateOfBirth(),
                account = Account.Null()
            };
        }

        public void OpenAccount(AccountNumber accountNumber, BillingDate billingDate)
        {
            if (account != null)
            {
                throw DomainError.Named("Account-Exists", "The account alreay exists");
            }
            
            account = Account.Open(accountNumber, billingDate);
        }

        public void CorrectDateOfBirth(DateOfBirth birthDate)
        {
            if (birthDate.GetCurrentAge() < 18)
            {
                throw DomainError.Named("Under-Age", "A client must be older than 18 years.");
            }

            dateOfBirth = birthDate;
        }

        protected override IMemento GetSnapshot()
        {
            return new ClientSnapshot
            {
                DateOfBirth = dateOfBirth,
                ClientName = clientName,
                PrimaryContactNumber = primaryContactNumber,
                AccountSnapshot = (account as IEntity).GetSnapshot() as AccountSnapshot
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (ClientSnapshot)memento;

            dateOfBirth = snapshot.DateOfBirth;
            clientName = snapshot.ClientName;
            primaryContactNumber = snapshot.PrimaryContactNumber;
        }
    }
}