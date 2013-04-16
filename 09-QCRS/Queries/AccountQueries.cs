using System.Collections.Generic;
using System.Linq;
using Domain.Core.Infrastructure;
using PersistenceModel.Reporting;
using Queries.Dtos;

namespace Queries
{
    public class AccountQueries
    {
        private readonly IDataQuery dataQuery;

        public AccountQueries(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public ICollection<AccountStatusHistoryDto> FetchStatusHistory(string accountNumber)
        {
            return dataQuery.GetQueryable<AccountStatusHistoryView>()
                            .Where(view => view.AccountNumber == accountNumber)
                            .ToList()
                            .ConvertAll(AccountStatusHistoryDto.BuildFromModel);
        }
    }
}