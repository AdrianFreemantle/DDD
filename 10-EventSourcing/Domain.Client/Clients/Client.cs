using System;
using System.Linq;
using Domain.Client.Accounts;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{
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
            if (AccountNumber == AccountNumber.Empty)
            {
                throw DomainError.Named("no-account", "The client may not be issued a loyalty card as they do not have an account.");
            }

            if (HasActiveLoyaltyCard())
            {
                throw DomainError.Named("active-loyalty-card", "The client may not be issued a loyalty card as they already have an active one.");
            }

            var loyaltyCardNumber = new LoyaltyCardNumber(Guid.NewGuid());
            RaiseEvent(new IssuedLoyaltyCard(Identity, loyaltyCardNumber, AccountNumber));
        }

        public void ReportLoyaltyCardAsStolen()
        {
            if (!HasActiveLoyaltyCard())
            {
                throw DomainError.Named("no-active-loyalty-card", "The client does not have an active loyalty card.");
            }

            loyaltyCards.Last().ReportLoyaltyCardAsStolen();
        }

        public void CancelLoyaltyCard()
        {
            if (!HasActiveLoyaltyCard())
            {
                throw DomainError.Named("no-active-loyalty-card", "The client does not have an active loyalty card.");
            }

            loyaltyCards.Last().CancelLoyaltyCard();
        }

        public bool HasActiveLoyaltyCard()
        {
            return loyaltyCards.Any() && loyaltyCards.Last().IsActive();
        }

        public Account OpenAccount(IAccountNumberService accountNumberService)
        {
            if (!ClientMayOpenAccount())
            {
                throw DomainError.Named("may-not-open-account", "The client is not elligible for oppening an account.");
            }

            Account account = Account.Open(Identity, accountNumberService.GetNextAccountNumber());
            RaiseEvent(new AccountAssingedToClient(Identity, account.Identity));
            return account;
        }

        public bool ClientMayOpenAccount()
        {
            var clientAge = dateOfBirth.GetCurrentAge();

            return (!isDeceased) && (clientAge >= 18) && AccountNumber.IsEmpty();
        }

        public void ClientIsDeceased()
        {
            RaiseEvent(new ClientPassedAway(Identity));
        }       
    }
}