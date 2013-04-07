using ApplicationService.Commands;
using Domain.Client.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Commands
{
    class CancelAccountConsoleCommand : CancelAccount, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "CancelAccount" }; }
        }

        public string Usage
        {
            get { return "CancelAccount <AccountNumber>"; }
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
