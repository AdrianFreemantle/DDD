using Domain.Client.Accounts;
using Domain.Client.Clients.Events;
using Domain.Core;

namespace Domain.Client.Clients
{
    public class LoyaltyCard : Entity<LoyaltyCardNumber>
    {
        private readonly ClientId clientId;
        private bool isActive;

        internal LoyaltyCard(ClientId clientId, LoyaltyCardNumber cardNumber)
        {
            isActive = true;
            Identity = cardNumber;
            this.clientId = clientId;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void ReportLoyaltyCardAsStolen()
        {
            RaiseEvent(new LoyaltyCardWasReportedStolen(clientId, Identity));
        }  
 
        public void CancelLoyaltyCard()
        {
            RaiseEvent(new LoyaltyCardWasCancelled(clientId, Identity));
        }

        public void When(LoyaltyCardWasReportedStolen @event)
        {
            isActive = false;
        }

        public void When(LoyaltyCardWasCancelled @event)
        {
            isActive = false;
        }
    }
}