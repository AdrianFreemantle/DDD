using Domain.Client.Accounts;
using Domain.Client.Clients.Events;
using Domain.Core;

namespace Domain.Client.Clients
{
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

        public void When(LoyaltyCardWasReportedStolen @event)
        {
            isDisabled = true;
        }
    }
}