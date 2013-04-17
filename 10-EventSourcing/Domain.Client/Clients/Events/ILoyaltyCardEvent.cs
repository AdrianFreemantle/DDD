using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    public interface ILoyaltyCardEvent : IDomainEvent
    {
        LoyaltyCardNumber CardNumber { get; }
    }
}