using System;
using System.Collections.Generic;

using Queries;
using Queries.Dtos;

namespace Shell.ConsoleViews
{
    public class ClientCardsConsoleView : IConsoleView
    {
        private readonly LoyaltyCardQueries loyaltyCardQueries;

        public string Key { get { return "ClientCards"; } }
        public string Usage { get { return "ClientCards <ClientId>"; } }

        public ClientCardsConsoleView(LoyaltyCardQueries loyaltyCardQueries)
        {
            this.loyaltyCardQueries = loyaltyCardQueries;
        }

        public void Print(string[] args)
        {
            if (args.Length != 1)
            {
                throw new Exception(String.Format("Error. Usage is: {0}", Usage));
            }

            ICollection<LoyaltyCardDto> cancelledCards = loyaltyCardQueries.FetchClientLoyaltyCards(args[0]);

            PrintBorder();
            Console.WriteLine("{0,-14} {1, -37} {2, -10} {3, -10}", "Client Id", "Card Number", "Stolen", "Cancelled");
            PrintBorder();

            foreach (var card in cancelledCards)
            {
                Console.WriteLine("{0,-14} {1, -37} {2, -10} {3, -10}", card.ClientId, card.CardNumber, card.Stolen, card.Cancelled);
            }

            PrintBorder();
        }

        private static void PrintBorder()
        {
            Console.WriteLine("=======================================================================================");
        }
    }
}