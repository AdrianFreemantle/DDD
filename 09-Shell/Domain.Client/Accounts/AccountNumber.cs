using Domain.Core;

namespace Domain.Client.Accounts
{
    public sealed class AccountNumber : IdentityBase<string>
    {
        public override string Id { get; protected set; }

        public AccountNumber(string accountNumber)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");

            Id = accountNumber;
        }

        public override string GetTag()
        {
            return "account-number";
        }
    }
}