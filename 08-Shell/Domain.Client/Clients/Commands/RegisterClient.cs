using Domain.Client.ValueObjects;
using Domain.Core.Commands;

namespace Domain.Client.Clients.Commands
{
    public class RegisterClient : ICommand
    {
        public IdentityNumber IdentityNumber { get; set; }
        public PersonName ClientName { get; set; }
        public TelephoneNumber PrimaryContactNumber { get; set; }
    }
}