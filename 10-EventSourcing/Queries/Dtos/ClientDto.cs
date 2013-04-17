using System;
using PersistenceModel.Reporting;

namespace Queries.Dtos
{
    public class ClientDto
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PrimaryContactNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDeceased { get; set; }
        public string AccountNumber { get; set; }
        public string AccountStatus { get; set; }
        public int AccountRecency { get; set; }

        public static ClientDto BuildFromView(ClientView view)
        {
            return new ClientDto
            {
                IdentityNumber = view.IdentityNumber,
                AccountNumber = view.AccountNumber,
                AccountRecency = view.AccountRecency,
                AccountStatus = view.AccountStatus.Description,
                DateOfBirth = view.DateOfBirth,
                FirstName = view.FirstName,
                Surname = view.Surname,
                IsDeceased = view.IsDeceased,
                PrimaryContactNumber = view.PrimaryContactNumber
            };
        }
    }
}
