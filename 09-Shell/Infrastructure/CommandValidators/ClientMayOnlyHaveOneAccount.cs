using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Domain.Client.Accounts.Commands;
using Domain.Core.Infrastructure;
using PersistenceModel;
using Domain.Core.Commands;
using Domain.Client.Clients;

namespace Infrastructure.CommandValidators
{
    public sealed class ClientMayOnlyHaveOneAccount : IValidateCommand<OpenAccount>
    {
        private readonly IDataQuery dataQuery;

        public ClientMayOnlyHaveOneAccount(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public IEnumerable<ValidationResult> Validate(OpenAccount command)
        {
            if (ClientHasAccount(command.ClientId))
            {
                return new[] { new ValidationResult("A client may not have more than one account.") };
            }

            return new ValidationResult[0];
        }

        public bool ClientHasAccount(ClientId clientId)
        {
            return dataQuery
                .GetQueryable<AccountModel>()
                .Any(account => account.ClientId == clientId.Id);
        }
    }
}