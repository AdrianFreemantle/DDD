using Domain.Core;

namespace Domain.Client.Clients
{
    public sealed class AccountNumber : IdentityBase<string>
    {
        public override string Id { get; protected set; }

        public AccountNumber(string accountNumber)
        {
            Id = accountNumber;
        }

        public override string GetTag()
        {
            return "account-number";
        }
    }
}