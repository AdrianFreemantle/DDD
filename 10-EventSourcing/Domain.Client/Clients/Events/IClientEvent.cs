using Domain.Core.Events;

namespace Domain.Client.Clients.Events
{
    public interface IClientEvent : IDomainEvent
    {
        ClientId ClientId { get; }
    }
}