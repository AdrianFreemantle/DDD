using Domain.Core;

namespace Domain.Client.Accounts
{
    public sealed class AccountNumber : IdentityBase<string>
    {
        public override string Id { get; protected set; }
        public bool IsEmpty { get { return string.IsNullOrWhiteSpace(Id); } }

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