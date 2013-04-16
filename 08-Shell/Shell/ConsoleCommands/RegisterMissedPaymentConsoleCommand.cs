using Domain.Client.Accounts;
using System;
using Domain.Client.Accounts.Commands;

namespace Shell.ConsoleCommands
{
    class RegisterMissedPaymentConsoleCommand : RegisterMissedPayment, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "RegisterMissedPayment" }; }
        }

        public string Usage
        {
            get { return "RegisterMissedPayment <AccountNumber>"; }
        }

        public void Build(string[] args)
        {
            if (args.Length != 1)
            {
                throw new Exception(String.Format("Error. Usage is: {0}", Usage));
            }

            AccountNumber = new AccountNumber(args[0]);
        }
    }
}
