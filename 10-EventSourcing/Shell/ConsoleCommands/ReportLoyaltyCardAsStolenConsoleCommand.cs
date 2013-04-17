using System;

using Domain.Client.Clients;
using Domain.Client.Clients.Commands;

namespace Shell.ConsoleCommands
{
    class ReportLoyaltyCardAsStolenConsoleCommand : ReportLoyaltyCardAsStolen, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "CardStolen" }; }
        }

        public string Usage
        {
            get { return "CardStolen <ClientId>"; }
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