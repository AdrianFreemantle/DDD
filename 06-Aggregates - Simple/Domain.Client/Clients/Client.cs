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
            };
        }

        public void OpenAccount(AccountNumber accountNumber)
        {
            if (account != null)
            {
                throw DomainError.Named("account-exists", "The client already has an account.");
            }
            
            account = Account.Open(accountNumber);
        }

        public void CorrectDateOfBirth(DateOfBirth birthDate)
        {
            if (birthDate.GetCurrentAge() < 18)
            {
                throw DomainError.Named("underage", "A client must be older than 18 years.");
            }

            dateOfBirth = birthDate;
        }

        public void RegisterPayment(BillingResult billingResult)
        {
            Mandate.ParameterNotNull(billingResult, "billingResult");

            if (account == null)
            {
                throw DomainError.Named("no-account", "The client does not yet have an account.");
            }

            account.RegisterPayment(billingResult);
        }

        public void ReversePayment(BillingResult billingResult)
        {
            Mandate.ParameterNotNull(billingResult, "billingResult");

            if (account == null)
            {
                throw DomainError.Named("no-account", "The client does not yet have an account.");
            }

            account.ReversePayment(billingResult);
        }

        protected override IMemento GetSnapshot()
        {
            AccountSnapshot accountSnapshot = null; 

            if (account != null)
            {
                accountSnapshot = (AccountSnapshot)(account as IEntity).GetSnapshot();
            }

            return new ClientSnapshot
            {
                DateOfBirth = dateOfBirth,
                ClientName = clientName,
                PrimaryContactNumber = primaryContactNumber,
                IdentityNumber = identityNumber,
                AccountSnapshot = accountSnapshot
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