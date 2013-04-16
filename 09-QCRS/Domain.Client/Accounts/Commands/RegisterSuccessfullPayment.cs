using System;
using System.Runtime.Serialization;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Commands;

namespace Domain.Client.Accounts.Commands
{
    [DataContract]
    public class RegisterSuccessfullPayment : ICommand
    {
        [DataMember(Order = 1, IsRequired = true, Name = "AccountNumber")]
        public AccountNumber AccountNumber { get; protected set; }

        [DataMember(Order = 2, IsRequired = true, Name = "BillingResult")]
        public BillingResult BillingResult { get; protected set; }

        protected RegisterSuccessfullPayment()
        {
        }

        public RegisterSuccessfullPayment(AccountNumber accountNumber, BillingResult billingResult)
        {
            Mandate.ParameterNotNullOrEmpty(accountNumber, "accountNumber");
            Mandate.ParameterNotDefaut(billingResult, "billingResult");

            AccountNumber = accountNumber;
            BillingResult = billingResult;
        }

        public override string ToString()
        {
            return String.Format("Registering account {0} payment of R{1}.", AccountNumber.Id, BillingResult.Amount);
        }
    }
}
