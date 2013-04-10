using Domain.Client.Accounts;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{
    public interface IClientSnapshot : IMemento
    {
        PersonName ClientName { get; set; }
        TelephoneNumber PrimaryContactNumber { get; set; }
        DateOfBirth DateOfBirth { get; set; }
        IdentityNumber IdentityNumber { get; set; }
        bool IsDeceased { get; set; }
    }
}