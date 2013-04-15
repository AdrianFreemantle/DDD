using System.Collections.Generic;
using System.Linq;
using Domain.Core.Infrastructure;
using PersistenceModel.Reporting;

namespace Queries
{
    public class ClientQueries
    {
        private readonly IDataQuery dataQuery;

        public ClientQueries(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public ICollection<ClientViewDto> FetchAllClients()
        {
            return dataQuery.GetQueryable<ClientView>()
                            .ToList()
                            .ConvertAll(ClientViewDto.BuildFromModel);

        }

        public ICollection<ClientViewDto> FetchDeceasedClients()
        {
            return dataQuery.GetQueryable<ClientView>()
                            .Where(view => view.IsDeceased)
                            .ToList()
                            .ConvertAll(ClientViewDto.BuildFromModel);
        }
    }
}