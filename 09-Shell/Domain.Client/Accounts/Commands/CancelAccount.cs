using Domain.Core.Commands;

namespace Domain.Client.Accounts.Commands
{
    public class CancelAccount : ICommand
    {
        public AccountNumber AccountNumber { get; set; }
    }
}