using System;

using PersistenceModel.Reporting;

namespace Queries.Dtos
{
    public class LoyaltyCardDto
    {
        public string ClientId { get; set; }
        public Guid CardNumber { get; set; }
        public bool Stolen { get; set; }
        public bool Cancelled { get; set; }

        public static LoyaltyCardDto BuildFromView(ClientLoyaltyCardView view)
        {
            return new LoyaltyCardDto
            {
                ClientId = view.ClientId,
                CardNumber = view.CardNumber,
                Cancelled = view.Cancelled,
                Stolen = view.Stolen
            };
        }
    }
}