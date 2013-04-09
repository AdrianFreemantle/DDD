using Domain.Client.Accounts;
using Domain.Client.Accounts.Commands;
using Domain.Client.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Commands
{
    class RegisterSuccessfullPaymentConsoleCommand : RegisterSuccessfullPayment, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "RegisterPayment" }; }
        }

        public string Usage
        {
            get { return "RegisterPayment <AccountNumber>"; }
        }

        public void Build(string[] args)
        {
            if (args.Length != 1)
            {
                throw new Exception(String.Format("Error. Usage is: {0}", Usage));
            }

            AccountNumber = new AccountNumber(args[0]);
            BillingResult = BillingResult.PaymentMade(Decimal.Parse(args[1]), DateTime.Today);
        }
    }
}
