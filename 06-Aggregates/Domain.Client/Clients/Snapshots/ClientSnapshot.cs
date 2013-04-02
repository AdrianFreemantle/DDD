using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients.Snapshots
{
    public interface IClientSnapshot : IMemento
    {
        PersonName ClientName { get; }
        TelephoneNumber PrimaryContactNumber { get; }
        DateOfBirth DateOfBirth { get; set; }
        IAccountSnapshot AccountSnapshot { get; }
    }
}