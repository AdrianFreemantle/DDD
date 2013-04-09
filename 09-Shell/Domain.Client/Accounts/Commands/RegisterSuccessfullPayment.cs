using Domain.Client.ValueObjects;
using Domain.Core.Commands;

namespace Domain.Client.Accounts.Commands
{
    public class RegisterSuccessfullPayment : ICommand
    {
        public AccountNumber AccountNumber { get; set; }
        public BillingResult BillingResult { get; set; }
    }
}
