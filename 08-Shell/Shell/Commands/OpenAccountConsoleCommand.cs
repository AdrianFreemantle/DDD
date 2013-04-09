using Domain.Client.Accounts.Commands;
using Domain.Client.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Commands
{
    class OpenAccountConsoleCommand : OpenAccount, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "OpenAccount" }; }
        }

        public string Usage
        {
            get { return "OpenAccount <ClientId>"; }
        }

        public void Build(string[] args)
        {
            if (args.Length != 1)
            {
                throw new Exception(String.Format("Error. Usage is: {0}", Usage));
            }

            ClientId = new ClientId(args[0]);
        }
    }
}
