using Domain.Client.Accounts;
using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    public class ClientIssuedLoyaltyCard : DomainEvent
    {
        public LoyaltyCardNumber LoyaltyCardNumber { get; set; }
        public AccountNumber AccountNumber { get; set; }

        public ClientIssuedLoyaltyCard(LoyaltyCardNumber loyaltyCardNumber, AccountNumber accountNumber)
        {
            LoyaltyCardNumber = loyaltyCardNumber;
            AccountNumber = accountNumber;
        }
    }
}