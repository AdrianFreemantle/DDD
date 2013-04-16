using System;
using System.ComponentModel.DataAnnotations;

namespace PersistenceModel.Reporting
{
    public class AccountStatusHistoryView
    {
        [Key]
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public AccountStatusLookup AccountStatus { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}