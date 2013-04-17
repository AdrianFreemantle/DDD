using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Client.Accounts;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;

namespace Domain.Client.Clients
{
    public sealed class LoyaltyCardNumber : IdentityBase<Guid>
    {
        public LoyaltyCardNumber(Guid cardNumber)
        {
            Mandate.ParameterNotDefaut(cardNumber, "cardNumber");

            Id = cardNumber;
        }

        public override string GetTag()
        {
            return "loyaltyCard";
        }

        public override bool IsEmpty()
        {
            return Id == Guid.Empty;
        }
    }

    public class LoyaltyCard : Entity<LoyaltyCardNumber>
    {
        private AccountNumber accountNumber;
        private bool isDisabled;

        internal LoyaltyCard(LoyaltyCardNumber cardNumber, AccountNumber accountNumber)
        {
            Identity = cardNumber;
            this.accountNumber = accountNumber;
        }

        public bool IsDisabled()
        {
            return isDisabled;
        }

        public void BankCardIsReportedStolen()
        {
            RaiseEvent(new LoyaltyCardWasReportedStolen(Identity));
        }   
    }

    public interface ILoyaltyCardEvent : IDomainEvent
    {
        LoyaltyCardNumber CardNumber { get; }
    }

    public class LoyaltyCardWasReportedStolen : DomainEvent, ILoyaltyCardEvent
    {
        public LoyaltyCardNumber CardNumber { get; protected set; }

        public LoyaltyCardWasReportedStolen(LoyaltyCardNumber identity)
        {
            CardNumber = identity;
        }
    }

    public partial class Client : Aggregate<ClientId>
    {       
        protected Client()
        {
        }

        public static Client RegisterClient(IdentityNumber idNumber, PersonName clientName, TelephoneNumber primaryContactNumber)
        {
            Mandate.ParameterNotDefaut(idNumber, "idNumber");
            Mandate.ParameterNotDefaut(clientName, "clientName");
            Mandate.ParameterNotDefaut(primaryContactNumber, "primaryContactNumber");

            var client = new Client();
            client.RaiseEvent(new ClientRegistered(new ClientId(idNumber), idNumber, clientName, primaryContactNumber));
            return client;
        }

        public void CorrectDateOfBirth(DateOfBirth birthDate)
        {
            Mandate.ParameterNotDefaut(birthDate, "birthDate");

            if (birthDate.GetCurrentAge() < 18)
            {
                throw DomainError.Named("underage", "A client must be older than 18 years.");
            }

            RaiseEvent(new ClientDateOfBirthCorrected(Identity, birthDate));
        }

        public void IssueLoyaltyCard()
        {
            if (accountNumber == AccountNumber.Empty)
            {
                throw DomainError.Named("no-account", "The client may not be issued a loyalty card as they do not have an account.");
            }

            if (loyaltyCards.Any(card => !card.IsDisabled()))
            {
                throw DomainError.Named("active-loyalty-card", "The client may not be issued a loyalty card as they already have an active one.");
            }

            RaiseEvent(new ClientIssuedLoyaltyCard(new LoyaltyCardNumber(Guid.NewGuid()), accountNumber));
        }

        public Account OpenAccount(IAccountNumberService accountNumberService)
        {
            if (!ClientMayOpenAccount())
            {
                throw DomainError.Named("may-not-open-account", "The client is not elligible for oppening an account.");
            }

            Account account = Account.Open(Identity, accountNumberService.GetNextAccountNumber());
            RaiseEvent(new AccountAssingedToClient(account.Identity));
            return account;
        }

        public bool ClientMayOpenAccount()
        {
            var clientAge = dateOfBirth.GetCurrentAge();

            return (!isDeceased) && (clientAge >= 18) && accountNumber.IsEmpty();
        }

        public void ClientIsDeceased()
        {
            RaiseEvent(new ClientPassedAway(Identity));
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