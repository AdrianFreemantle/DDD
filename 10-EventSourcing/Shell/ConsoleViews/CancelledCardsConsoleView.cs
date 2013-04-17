using System;
using System.Collections.Generic;

using Queries;
using Queries.Dtos;

namespace Shell.ConsoleViews
{
    public class CancelledCardsConsoleView : IConsoleView
    {
        private readonly LoyaltyCardQueries loyaltyCardQueries;

        public string Key { get { return "CancelledCards"; } }
        public string Usage { get { return "CancelledCards"; } }

        public CancelledCardsConsoleView(LoyaltyCardQueries loyaltyCardQueries)
        {
            this.loyaltyCardQueries = loyaltyCardQueries;
        }

        public void Print(string[] args)
        {
            ICollection<LoyaltyCardDto> cancelledCards = loyaltyCardQueries.FetchCancelledLoyaltyCards();

            PrintBorder();
            Console.WriteLine("{0,-14} {1, -15}", "Client Id", "Card Number");
            PrintBorder();

            foreach (var card in cancelledCards)
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