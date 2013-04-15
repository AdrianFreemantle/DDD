using System.ComponentModel.DataAnnotations;

namespace PersistenceModel.Reporting
{
    public class AccountStatusLookup
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}