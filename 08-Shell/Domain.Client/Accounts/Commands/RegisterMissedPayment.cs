using Domain.Core.Commands;

namespace Domain.Client.Accounts.Commands
{
    public class RegisterMissedPayment : ICommand
    {
        public AccountNumber AccountNumber { get; set; }
    }
}