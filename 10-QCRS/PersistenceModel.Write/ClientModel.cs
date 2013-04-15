using System;
using System.ComponentModel.DataAnnotations;

namespace PersistenceModel.Write
{
    public class ClientModel
    {
        [Key]
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PrimaryContactNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDeceased { get; set; }
        public string AccountNumber { get; set; }
    }
}   