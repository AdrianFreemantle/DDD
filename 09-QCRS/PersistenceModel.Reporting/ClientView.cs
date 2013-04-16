using System;
using System.ComponentModel.DataAnnotations;

namespace PersistenceModel.Reporting
{
    public class ClientView
    {
        [Key]
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PrimaryContactNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDeceased { get; set; }
        public string AccountNumber { get; set; }
        public AccountStatusLookup AccountStatus { get; set; }
        public int AccountRecency { get; set; }
    }
}
