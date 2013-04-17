using System;
using System.ComponentModel.DataAnnotations;

namespace PersistenceModel.Reporting
{
    public class ClientLoyaltyCardView
    {
        [Key]
        public Guid CardNumber { get; set; }
        public string ClientId { get; set; }
        public bool Stolen { get; set; }
        public bool Cancelled { get; set; }
    }
}