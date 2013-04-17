using System.Collections.Generic;
using System.Linq;

using Domain.Core.Infrastructure;

using PersistenceModel.Reporting;

using Queries.Dtos;

namespace Queries
{
    public class LoyaltyCardQueries
    {
        private readonly IDataQuery dataQuery;

        public LoyaltyCardQueries(IDataQuery dataQuery)
        {
            this.dataQuery = dataQuery;
        }

        public ICollection<LoyaltyCardDto> FetchClientLoyaltyCards(string clientId)
        {
            return dataQuery.GetQueryable<ClientLoyaltyCardView>()
                            .Where(view => view.ClientId == clientId)
                            .ToList()
                            .ConvertAll(LoyaltyCardDto.BuildFromView);
        }

        public ICollection<LoyaltyCardDto> FetchStolenLoyaltyCards()
        {
            return dataQuery.GetQueryable<ClientLoyaltyCardView>()
                            .Where(view => view.Stolen)
                            .ToList()
                            .ConvertAll(LoyaltyCardDto.BuildFromView);
        }

        public ICollection<LoyaltyCardDto> FetchCancelledLoyaltyCards()
        {
            return dataQuery.GetQueryable<ClientLoyaltyCardView>()
                            .Where(view => view.Cancelled)
                            .ToList()
                            .ConvertAll(LoyaltyCardDto.BuildFromView);
        }
    }
}