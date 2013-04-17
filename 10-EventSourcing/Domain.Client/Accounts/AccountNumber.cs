using System;
using Domain.Core;

namespace Domain.Client.Accounts
{
    public sealed class AccountNumber : IdentityBase<string>
    {
        public static AccountNumber Empty
        {
            get
            {
                return new AccountNumber
                {
                    Id = String.Empty
                };
            }
        }

        private AccountNumber()
        {
        }

        public AccountNumber(string accountNumber)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");

            Id = accountNumber;
        }

        public override string GetTag()
        {
            return "account";
        }

        public override bool IsEmpty()
        {
            return String.IsNullOrWhiteSpace(Id);
        }
    }
}