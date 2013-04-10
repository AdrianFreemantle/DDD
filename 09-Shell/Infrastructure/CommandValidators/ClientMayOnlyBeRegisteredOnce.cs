using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Domain.Client.Clients.Commands;
using Domain.Core.Infrastructure;
using PersistenceModel;
using Domain.Core.Commands;
using Domain.Client.ValueObjects;

namespace Infrastructure.DomainSpecifications
{
    public sealed class ClientMayOnlyBeRegisteredOnce : IValidateCommand<RegisterClient>
    {
        private readonly IDataQuery dataQuery;

        public ClientMayOnlyBeRegisteredOnce(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }       

        public IEnumerable<ValidationResult> Validate(RegisterClient command)
        {
            if (ClientIsRegistered(command.IdentityNumber))
            {
                return new[] { new ValidationResult("The client is already registered.") };
            }

            return new ValidationResult[0];
        }

        private bool ClientIsRegistered(IdentityNumber identityNumber)
        {
            return dataQuery
                .GetQueryable<ClientModel>()
                .Any(client => client.IdentityNumber == identityNumber.Number);
        }
    }
}
