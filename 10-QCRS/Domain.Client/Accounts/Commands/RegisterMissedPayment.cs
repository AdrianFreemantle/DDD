using System;
using System.Runtime.Serialization;
using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Accounts.Commands
{
    [DataContract]
    public class RegisterMissedPayment : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; protected set; }

        protected RegisterMissedPayment()
        {
        }

        public RegisterMissedPayment(AccountNumber accountNumber)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");

            AccountNumber = accountNumber;
        }

        public override string ToString()
        {
            return String.Format("Registering missed payment on account {0}.", AccountNumber.Id);
        }
    }
}