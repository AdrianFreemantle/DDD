using System;

using Domain.Client.Clients;
using Domain.Client.Clients.Commands;

namespace Shell.ConsoleCommands
{
    class IssueLoyaltyCardConsoleCommand : IssueLoyaltyCard, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "IssueCard" }; }
        }

        public string Usage
        {
            get { return "IssueCard <ClientId>"; }
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