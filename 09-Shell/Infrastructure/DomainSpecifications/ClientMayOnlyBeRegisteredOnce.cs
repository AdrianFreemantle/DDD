using System.Linq;
using Domain.Client.Clients.Commands;
using Domain.Client.DomainSpecifications;
using Domain.Core.Infrastructure;
using PersistenceModel;

namespace Infrastructure.DomainSpecifications
{
    public sealed class ClientMayOnlyBeRegisteredOnce : ClientMayOnlyBeRegisteredOnceRule
    {
        private readonly IDataQuery dataQuery;

        public ClientMayOnlyBeRegisteredOnce(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public override bool IsValid(RegisterClient command)
        {
            return dataQuery
                .GetQueryable<ClientModel>()
                .All(client => client.IdentityNumber != command.IdentityNumber.Number);
        }
    }
}
