using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Commands
{
    public class RegisterClient : ICommand
    {
        public IdentityNumber IdentityNumber { get; set; }
        public PersonName ClientName { get; set; }
        public TelephoneNumber PrimaryContactNumber { get; set; }
    }

    public class OpenAccount : ICommand
    {
        public ClientId ClientId { get; set; }
    }

    public class CorrectDateOfBirth : ICommand
    {
        public ClientId ClientId { get; set; }
        public DateOfBirth DateOfBirth { get; set; }
    }

    public class SetClientAsDeceased : ICommand
    {
        public ClientId ClientId { get; set; }
    }
}
