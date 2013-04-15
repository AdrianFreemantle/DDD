using System.ComponentModel.DataAnnotations;

namespace PersistenceModel.Write
{
    public class AccountModel
    {
        [Key]
        public string AccountNumber { get; set; }
        public string ClientId { get; set; }
        public int AccountStatusId { get; set; }
        public int Recency { get; set; }
    }
}