using System.Linq;
using Domain.Client.Clients.Commands;
using Domain.Core.Commands;
using Domain.Core.Infrastructure;
using PersistenceModel;

namespace Infrastructure.DomainSpecifications
{
    public sealed class ClientMayOnlyBeRegisteredOnce : ICommandSpecification<RegisterClient>
    {
        private readonly IDataQuery dataQuery;

        public string ErrorMessage { get { return "The client is already registered."; } }

        public ClientMayOnlyBeRegisteredOnce(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public bool IsValid(RegisterClient command)
        {
            return dataQuery
                .GetQueryable<ClientModel>()
                .All(client => client.IdentityNumber != command.IdentityNumber.Number);
        }
    }
}
