using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;

namespace Domain.Client.Clients
{
    public interface IClientRepository : IAggregateRepository<Client>
    {
        Client Get(IdentityNumber identityNumber);
    }
}