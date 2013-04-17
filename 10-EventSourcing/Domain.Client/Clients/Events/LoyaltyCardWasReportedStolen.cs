using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    public class LoyaltyCardWasReportedStolen : DomainEvent, ILoyaltyCardEvent
    {
        public LoyaltyCardNumber CardNumber { get; protected set; }

        public LoyaltyCardWasReportedStolen(LoyaltyCardNumber identity)
        {
            CardNumber = identity;
        }
    }
}