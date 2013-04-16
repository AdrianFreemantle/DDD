using System;
using System.Collections.Generic;
using Queries;
using Queries.Dtos;

namespace Shell.ConsoleViews
{
    public class AllClientsConsoleView : IConsoleView
    {
        private readonly ClientQueries clientQueries;

        public string Key { get { return "AllClients"; } }
        public string Usage { get { return "AllClients"; } }
       
        public AllClientsConsoleView(ClientQueries clientQueries)
        {
            this.clientQueries = clientQueries;
        }

        public void Print(string[] args)
        {
            ICollection<ClientDto> allClients = clientQueries.FetchAllClients();

            PrintBorder();
            Console.WriteLine("{0,-14} {1, -15} {2, -15} {3, -11} {4, -8} {5, -11} {6, -8}", "Id Number", "Surname", "Name", "Telephone", "Acc #", "Acc Status", "Recency");
            PrintBorder();

            foreach (var client in allClients)
            {
                Console.WriteLine("{0,-14} {1, -15} {2, -15} {3, -11} {4, -8} {5, -11} {6, -8}", client.IdentityNumber, client.Surname, client.FirstName, client.PrimaryContactNumber, client.AccountNumber, client.AccountStatus, client.AccountRecency);
            }

            PrintBorder();
        }

        private static void PrintBorder()
        {
            Console.WriteLine("=======================================================================================");
        }              
    }
}