using System;
using System.Collections.Generic;

using Queries;
using Queries.Dtos;

namespace Shell.ConsoleViews
{
    public class StolenCardsConsoleView : IConsoleView
    {
        private readonly LoyaltyCardQueries loyaltyCardQueries;

        public string Key { get { return "StolenCards"; } }
        public string Usage { get { return "StolenCards"; } }

        public StolenCardsConsoleView(LoyaltyCardQueries loyaltyCardQueries)
        {
            this.loyaltyCardQueries = loyaltyCardQueries;
        }

        public void Print(string[] args)
        {
            ICollection<LoyaltyCardDto> stolenCards = loyaltyCardQueries.FetchStolenLoyaltyCards();

            PrintBorder();
            Console.WriteLine("{0,-14} {1, -15}", "Client Id", "Card Number");
            PrintBorder();

            foreach (var card in stolenCards)
            {
                Console.WriteLine("{0,-14} {1, -15}", card.ClientId, card.CardNumber);
            }

            PrintBorder();
        }

        private static void PrintBorder()
        {
            Console.WriteLine("=======================================================================================");
        } 
    }
}