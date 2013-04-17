using System;

using Domain.Client.Clients;
using Domain.Client.Clients.Commands;

namespace Shell.ConsoleCommands
{
    class CancelLoyaltyCardConsoleCommand : CancelLoyaltyCard, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "CancelCard" }; }
        }

        public string Usage
        {
            get { return "CancelCard <ClientId>"; }
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