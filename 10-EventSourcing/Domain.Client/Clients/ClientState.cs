using System.Collections.Generic;
using System.Linq;
using Domain.Client.Accounts;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Clients
{
    public interface IHandleClientStateTransitions
    {
        void When(ClientRegistered @event);
        void When(ClientDateOfBirthCorrected @event);
        void When(ClientPassedAway @event);
        void When(AccountAssingedToClient @event);
        void When(ClientIssuedLoyaltyCard @event);
    }

    public partial class Client : IHandleClientStateTransitions
    {
        private IdentityNumber identityNumber;
        private AccountNumber accountNumber;
        private PersonName clientName;
        private TelephoneNumber primaryContactNumber;
        private DateOfBirth dateOfBirth;
        private bool isDeceased;
        private readonly HashSet<LoyaltyCard> loyaltyCards = new HashSet<LoyaltyCard>();  

        void IHandleClientStateTransitions.When(ClientRegistered @event)
        {
            accountNumber = AccountNumber.Empty;
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

        void IHandleClientStateTransitions.When(AccountAssingedToClient @event)
        {
            accountNumber = @event.AccountNumber;
        }

        void IHandleClientStateTransitions.When(ClientIssuedLoyaltyCard @event)
        {
            var card = new LoyaltyCard(@event.LoyaltyCardNumber, @event.AccountNumber);
            ((IEntity)card).SaveChangesHandler = SaveChange;
            loyaltyCards.Add(card);
        }

        protected override void ApplyEvent(object @event)
        {
            if (@event is ILoyaltyCardEvent)
            {
                var loyaltyCard = loyaltyCards.Single(card => card.Identity == ((ILoyaltyCardEvent)@event).CardNumber);
                ((IEntity)loyaltyCard).ApplyEvent(@event);
            }
            else
            {
                ((IHandleClientStateTransitions)this).When((dynamic)@event);
            }
        }
    }
}