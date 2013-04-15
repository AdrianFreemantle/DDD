using Domain.Client.Clients;
using Domain.Client.Clients.Commands;
using Domain.Client.ValueObjects;
using System;

namespace Shell.ConsoleCommands
{
    class CorrectDateOfBirthConsoleCommand : CorrectDateOfBirth, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "CorrectDateOfBirth" }; }
        }

        public string Usage
        {
            get { return "CorrectDateOfBirth <ClientId> <Date> { Date is YYYY-MM-DD }"; }
        }

        public void Build(string[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception(String.Format("Error. Usage is: {0}", Usage));
            }

            ClientId = new ClientId(args[0]);
            DateOfBirth = new DateOfBirth(DateTime.Parse(args[1]));
        }
    }
}
