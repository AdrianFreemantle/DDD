using Domain.Client.Accounts;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;

namespace Domain.Client.Clients
{
    public interface IHandleClientStateTransitions
    {
        void When(ClientRegistered @event);
        void When(ClientDateOfBirthCorrected @event);
        void When(ClientPassedAway @event);
    }

    public partial class Client : IHandleClientStateTransitions
    {
        private IdentityNumber identityNumber;
        private PersonName clientName;
        private TelephoneNumber primaryContactNumber;
        private DateOfBirth dateOfBirth;
        private bool isDeceased;

        void IHandleClientStateTransitions.When(ClientRegistered @event)
        {
            Identity = @event.ClientId;
            identityNumber = @event.IdentityNumber;
            clientName = @event.ClientName;
            primaryContactNumber = @event.PrimaryContactNumber;
            dateOfBirth = @event.IdentityNumber.GetDateOfBirth();
        }

        void IHandleClientStateTransitions.When(ClientDateOfBirthCorrected @event)
        {
            dateOfBirth = @event.DateOfBirth;
        }

        void IHandleClientStateTransitions.When(ClientPassedAway @event)
        {
            isDeceased = true;
        }

        protected override void ApplyEvent(object @event)
        {
            ((IHandleClientStateTransitions)this).When((dynamic)@event);
        }
    }
}