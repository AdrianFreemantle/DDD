using Domain.Client.Accounts;

namespace Domain.Client.Clients.Services
{
    public interface IAccountNumberService
    {
        AccountNumber GetNextAccountNumber();
    }
}
