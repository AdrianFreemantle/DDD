using System.Linq;
using Domain.Client.Clients.Commands;
using Domain.Core.Infrastructure;
using PersistenceModel;
using Domain.Core.Commands;

namespace Infrastructure.DomainSpecifications
{
    public sealed class ClientMayOnlyBeRegisteredOnce : ICommandValidation<RegisterClient>
    {
        public string ErrorMessage { get { return "The client is already registered."; } }

        private readonly IDataQuery dataQuery;

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
