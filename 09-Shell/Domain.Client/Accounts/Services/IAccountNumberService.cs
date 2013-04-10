using Domain.Client.Clients;

namespace Domain.Client.Accounts.Services
{
    public interface IAccountNumberService
    {
        AccountNumber GetNextAccountNumber();
        AccountNumber GetAccountNumberForClient(ClientId clientId);
    }
}
