using System;

namespace PersistenceModel
{
    public class ClientModel
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PrimaryContactNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDeceased { get; set; }
        public string AccountNumber { get; set; }
    }

    public class AccountModel
    {
        public string AccountNumber { get; set; }
        public string ClientId { get; set; }
        public int AccountStatusId { get; set; }
        public int Recency { get; set; }
    }
}   