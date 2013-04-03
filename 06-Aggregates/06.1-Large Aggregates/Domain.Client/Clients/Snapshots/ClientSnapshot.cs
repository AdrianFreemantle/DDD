using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients.Snapshots
{
    public class ClientSnapshot : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public PersonName ClientName { get; set; }
        public TelephoneNumber PrimaryContactNumber { get; set; }
        public DateOfBirth DateOfBirth { get; set; }
        public IdentityNumber IdentityNumber { get; set; }
        public AccountSnapshot AccountSnapshot { get; set; }
    }
}