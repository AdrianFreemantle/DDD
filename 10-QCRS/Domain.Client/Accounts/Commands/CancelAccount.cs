using System;
using System.Runtime.Serialization;
using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Accounts.Commands
{
    [DataContract]
    public class CancelAccount : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; protected set; }

        protected CancelAccount()
        {
        }

        public CancelAccount(AccountNumber accountNumber)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");

            AccountNumber = accountNumber;
        }

        public override string ToString()
        {
            return String.Format("Cancelling account {0}", AccountNumber.Id);
        }
    }
}