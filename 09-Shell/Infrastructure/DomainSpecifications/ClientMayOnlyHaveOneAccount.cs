using System.Linq;
using Domain.Client.Accounts.Commands;
using Domain.Client.DomainSpecifications;
using Domain.Core.Infrastructure;
using PersistenceModel;

namespace Infrastructure.DomainSpecifications
{
    public sealed class ClientMayOnlyHaveOneAccount : ClientMayOnlyHaveOneAccountRule
    {
        private readonly IDataQuery dataQuery;

        public ClientMayOnlyHaveOneAccount(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public override bool IsValid(OpenAccount command)
        {
            return dataQuery
                .GetQueryable<AccountModel>()
                .All(account => account.ClientId != command.ClientId.Id);
        }
    }
}