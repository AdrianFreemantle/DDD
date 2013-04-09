using Domain.Client.Clients;
using Domain.Core.Commands;

namespace Domain.Client.Accounts.Commands
{
    public class OpenAccount : ICommand
    {
        public ClientId ClientId { get; set; }
    }
}