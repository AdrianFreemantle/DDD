using System;
using Domain.Core;

namespace Domain.Client.Accounts
{
    public sealed class AccountNumber : IdentityBase<string>
    {
        public override string Id { get; protected set; }

        private AccountNumber()
        {
        }

        public AccountNumber(string accountNumber)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");

            Id = accountNumber;
        }

        public static AccountNumber Empty()
        {
            return new AccountNumber
            {
                Id = String.Empty
            };
        }

        public override string GetTag()
        {
            return "account-number";
        }

        public override bool IsEmpty()
        {
            return String.IsNullOrWhiteSpace(Id);
        }
    }
}