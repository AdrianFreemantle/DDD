using Domain.Client.ValueObjects;
using Domain.Core.Commands;

namespace Domain.Client.Clients.Commands
{
    public class CorrectDateOfBirth : ICommand
    {
        public ClientId ClientId { get; set; }
        public DateOfBirth DateOfBirth { get; set; }
    }
}