using Domain.Core.Commands;

namespace Domain.Client.Clients.Commands
{
    public class SetClientAsDeceased : ICommand
    {
        public ClientId ClientId { get; set; }
    }
}
