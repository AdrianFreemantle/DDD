using Domain.Client.Clients.Events;

namespace Domain.Client.Clients
{
    public interface IClientEvents
    {
        void When(ClientDateOfBirthCorrected @event);
        void When(ClientRegistered @event);
    }
}