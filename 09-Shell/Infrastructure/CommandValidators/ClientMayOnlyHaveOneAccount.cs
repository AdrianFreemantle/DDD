using System.Linq;
using Domain.Client.Accounts.Commands;
using Domain.Core.Infrastructure;
using PersistenceModel;
using Domain.Core.Commands;

namespace Infrastructure.CommandValidators
{
    public sealed class ClientMayOnlyHaveOneAccount : ICommandValidation<OpenAccount>
    {
        public string ErrorMessage { get { return "A client may not have more than one account."; } }

        private readonly IDataQuery dataQuery;

        public ClientMayOnlyHaveOneAccount(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public bool IsValid(OpenAccount command)
        {
            return dataQuery
                .GetQueryable<AccountModel>()
                .All(account => account.ClientId != command.ClientId.Id);
        }
    }
}