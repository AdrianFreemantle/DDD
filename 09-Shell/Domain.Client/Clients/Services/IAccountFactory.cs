using Domain.Client.Accounts;

namespace Domain.Client.Clients.Services
{
    public interface IAccountFactory
    {
        Account OpenAccount(ClientId clientId);
    }
}
