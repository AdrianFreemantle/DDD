namespace Domain.Client.Clients.Services
{
    public interface IAccountCancellationService
    {
        void CancelClientAccount(ClientId clientId);
    }
}