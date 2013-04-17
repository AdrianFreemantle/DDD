using System.ComponentModel.DataAnnotations;
using Domain.Core.Infrastructure;

namespace PersistenceModel.Reporting
{
    public class AccountStatusLookup : ILookupTable 
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}