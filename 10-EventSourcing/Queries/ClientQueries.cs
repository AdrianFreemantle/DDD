using System.Collections.Generic;
using System.Linq;
using Domain.Core.Infrastructure;
using PersistenceModel.Reporting;
using Queries.Dtos;

namespace Queries
{
    public class ClientQueries
    {
        private readonly IDataQuery dataQuery;

        public ClientQueries(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public ICollection<ClientDto> FetchAllClients()
        {
            return dataQuery.GetQueryable<ClientView>()
                            .ToList()
                            .ConvertAll(ClientDto.BuildFromView);

        }

        public ICollection<ClientDto> FetchDeceasedClients()
        {
            return dataQuery.GetQueryable<ClientView>()
                            .Where(view => view.IsDeceased)
                            .ToList()
                            .ConvertAll(ClientDto.BuildFromView);
        }
    }
}