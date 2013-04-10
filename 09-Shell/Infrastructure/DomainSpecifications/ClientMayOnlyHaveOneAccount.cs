using System.Linq;
using Domain.Client.Accounts.Commands;
using Domain.Core.Commands;
using Domain.Core.Infrastructure;
using PersistenceModel;

namespace Infrastructure.DomainSpecifications
{
    public sealed class ClientMayOnlyHaveOneAccount : ICommandSpecification<OpenAccount>
    {
        private readonly IDataQuery dataQuery;

        public string ErrorMessage { get { return "A client may not have more than one account."; } }

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