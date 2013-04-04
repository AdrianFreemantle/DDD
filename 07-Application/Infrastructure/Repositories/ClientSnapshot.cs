using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Infrastructure.Repositories
{
    class ClientSnapshot : IClientSnapshot
    {
        public IHaveIdentity Identity { get; set; }
        public PersonName ClientName { get; set; }
        public TelephoneNumber PrimaryContactNumber { get; set; }
        public DateOfBirth DateOfBirth { get; set; }
        public IdentityNumber IdentityNumber { get; set; }
        public bool IsDeceased { get; set; }
    }
}
