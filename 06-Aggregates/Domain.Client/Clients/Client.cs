using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{
    public class Account : Entity<AccountNumber>
    {
        private Recency recency;
        private BillingDate billingDate;

        protected Account()
        {
        }

        internal static Account Open(AccountNumber accountNumber, BillingDate billingDate)
        {
            return new Account
            {
                billingDate = billingDate,
                Identity = accountNumber
            };
        }
    }

    public class Client : Aggregate<ClientId>, IClientEvents
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
            var client = new Client();
            client.RaiseEvent(new ClientRegistered(new ClientId(idNumber), idNumber, clientName, telephoneNumber));
            return client;
        }

        void IClientEvents.When(ClientRegistered @event)
        {
            Identity = @event.ClientId;
            identityNumber = @event.IdentityNumber;
            clientName = @event.ClientName;
            primaryContactNumber = @event.TelephoneNumber;
            dateOfBirth = identityNumber.GetDateOfBirth();
        }

        public void CorrectDateOfBirth(DateOfBirth dateOfBirth)
        {
            if (dateOfBirth.GetCurrentAge() < 18)
            {
                throw DomainError.Named("Under-Age", "A client must be older than 18 years.");
            }

            RaiseEvent(new ClientDateOfBirthCorrected(Identity.Id, dateOfBirth));
        }

        void IClientEvents.When(ClientDateOfBirthCorrected @event)
        {
            dateOfBirth = @event.DateOfBirth;
        }

        protected override IMemento GetSnapshot()
        {
            return new ClientSnapshot
            {
                DateOfBirth = dateOfBirth,
                ClientName = clientName,
                PrimaryContactNumber = primaryContactNumber
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (ClientSnapshot)memento;

            dateOfBirth = snapshot.DateOfBirth;
            clientName = snapshot.ClientName;
            primaryContactNumber = snapshot.PrimaryContactNumber;
        }

        protected override void ApplyEvent(object @event)
        {
            ((IClientEvents)this).When((dynamic)@event);
        }
    }
}