using Domain.Client.Clients.Events;
using Domain.Client.Clients.Snapshots;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{
    public partial class Client : IClientState
    {
        private IdentityNumber identityNumber;
        private PersonName clientName;
        private TelephoneNumber primaryContactNumber;
        private DateOfBirth dateOfBirth;

        private Account account;

        void IClientState.When(ClientRegistered @event)
        {
            Identity = @event.ClientId;
            identityNumber = @event.IdentityNumber;
            clientName = @event.ClientName;
            primaryContactNumber = @event.TelephoneNumber;
            dateOfBirth = identityNumber.GetDateOfBirth();
        }

        void IClientState.When(ClientDateOfBirthCorrected @event)
        {
            dateOfBirth = @event.DateOfBirth;
        }

        void IClientState.When(AccountOpened @event)
        {
            account.When(@event);
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (IClientSnapshot)memento;

            dateOfBirth = snapshot.DateOfBirth;
            clientName = snapshot.ClientName;
            primaryContactNumber = snapshot.PrimaryContactNumber;
        }

        protected override void ApplyEvent(object @event)
        {
            ((IClientState)this).When((dynamic)@event);
        }
    }
}
