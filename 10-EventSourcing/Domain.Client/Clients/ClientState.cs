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
        void When(IssuedLoyaltyCard @event);
    }

    public partial class Client : IHandleClientStateTransitions
    {
        public AccountNumber AccountNumber { get; private set; }

        private IdentityNumber identityNumber;
        private PersonName clientName;
        private TelephoneNumber primaryContactNumber;
        private DateOfBirth dateOfBirth;
        private bool isDeceased;
        private readonly HashSet<LoyaltyCard> loyaltyCards = new HashSet<LoyaltyCard>();  

        void IHandleClientStateTransitions.When(ClientRegistered @event)
        {
            AccountNumber = AccountNumber.Empty;
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
            AccountNumber = @event.AccountNumber;
        }

        void IHandleClientStateTransitions.When(IssuedLoyaltyCard @event)
        {
            var card = new LoyaltyCard(Identity, @event.CardNumber);
            ((IEntity)card).RegisterChangesHandler(SaveChange);
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

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (ClientSnapshot)memento;

            dateOfBirth = snapshot.DateOfBirth;
            clientName = snapshot.ClientName;
            primaryContactNumber = snapshot.PrimaryContactNumber;
            identityNumber = snapshot.IdentityNumber;
            isDeceased = snapshot.IsDeceased;
        }

        protected override IMemento GetSnapshot()
        {
            return new ClientSnapshot
            {
                ClientName = clientName,
                DateOfBirth = dateOfBirth,
                PrimaryContactNumber = primaryContactNumber,
                Identity = Identity,
                IdentityNumber = identityNumber,
                IsDeceased = isDeceased
            };
        } 
    }
}