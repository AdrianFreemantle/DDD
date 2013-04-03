using Domain.Client.Events;
using Domain.Client.ValueObjects;

namespace Domain.Client.Clients
{
    interface IClientState
    {
        void When(ClientRegistered @event);
        void When(ClientDateOfBirthCorrected @event);
        void When(ClientPassedAway @event);
    }

    public partial class Client : IClientState
    {
        private IdentityNumber identityNumber;
        private PersonName clientName;
        private TelephoneNumber primaryContactNumber;
        private DateOfBirth dateOfBirth;
        private bool isDeceased;

        void IClientState.When(ClientRegistered @event)
        {
            Identity = @event.ClientId;
            identityNumber = @event.IdentityNumber;
            clientName = @event.ClientName;
            primaryContactNumber = @event.PrimaryContactNumber;
            dateOfBirth = @event.IdentityNumber.GetDateOfBirth();
        }

        void IClientState.When(ClientDateOfBirthCorrected @event)
        {
            dateOfBirth = @event.DateOfBirth;
        }

        void IClientState.When(ClientPassedAway @event)
        {
            isDeceased = true;
        }

        protected override void ApplyEvent(object @event)
        {
            ((IClientState)this).When((dynamic)@event);
        }
    }
}