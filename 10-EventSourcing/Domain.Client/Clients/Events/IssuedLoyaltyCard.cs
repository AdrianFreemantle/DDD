using Domain.Client.Accounts;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    public class IssuedLoyaltyCard : DomainEvent, ILoyaltyCardEvent
    {
        public LoyaltyCardNumber CardNumber { get; private set; }
        public AccountNumber AccountNumber { get; set; }

        public IssuedLoyaltyCard(LoyaltyCardNumber loyaltyCardNumber, AccountNumber accountNumber)
        {
            CardNumber = loyaltyCardNumber;
            AccountNumber = accountNumber;
        }

    }
}