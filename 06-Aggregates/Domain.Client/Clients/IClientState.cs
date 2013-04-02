using Domain.Client.Clients.Events;

namespace Domain.Client.Clients
{
    public interface IClientState
    {
        void When(ClientDateOfBirthCorrected @event);
        void When(ClientRegistered @event);
        void When(AccountOpened @event);
    }
}