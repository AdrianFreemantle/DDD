using Domain.Client.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Client.Clients.Commands;

namespace Shell.Commands
{
    class SetClientAsDeceasedConsoleCommand : SetClientAsDeceased, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "SetClientAsDeceased" }; }
        }

        public string Usage
        {
            get { return "SetClientAsDeceased <ClientId> {Mortalizes the client}"; }
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
