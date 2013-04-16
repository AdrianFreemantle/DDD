using System;
using System.Collections.Generic;
using Queries;
using Queries.Dtos;

namespace Shell.ConsoleViews
{
    public class AccountStatusHistoryConsoleView : IConsoleView
    {
        private readonly AccountQueries accountQueries;

        public string Key { get { return "AccHistory"; } }
        public string Usage { get { return "AccHistory <AccountNumber>"; } }

        public AccountStatusHistoryConsoleView(AccountQueries accountQueries)
        {
            this.accountQueries = accountQueries;
        }

        public void Print(string[] args)
        {
            ICollection<AccountStatusHistoryDto> history = accountQueries.FetchStatusHistory(args[0]);

            PrintBorder();
            Console.WriteLine("{0,-14} {1, -15}", "Changed Date", "Status");
            PrintBorder();

            foreach (AccountStatusHistoryDto entry in history)
            {
                Console.WriteLine("{0,-14} {1, -15}", entry.ChangedDate, entry.AccountStatus);
            }

            PrintBorder();
        }

        private static void PrintBorder()
        {
            Console.WriteLine("=======================================================================================");
        }        
    }
}