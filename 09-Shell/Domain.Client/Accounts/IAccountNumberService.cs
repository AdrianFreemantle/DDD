using Domain.Client.Clients;

namespace Domain.Client.Accounts
{
    public interface IAccountNumberService
    {
        AccountNumber GetNextAccountNumber();
        AccountNumber GetAccountNumberForClient(ClientId clientId);
    }
}
