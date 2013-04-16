using Domain.Core.Infrastructure;

namespace Domain.Client.Accounts
{
    public interface IAccountRepository : IAggregateRepository<Account>
    {
    }
}