using System;
using ApplicationService;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.Events;
using Domain.Core.Events;
using Domain.Core.Infrastructure;
using Infrastructure;
using Infrastructure.Repositories;
using PersistenceModel;
using Infrastructure.Services;
using ApplicationService.EventHandlers;
using System.Threading;
using System.Linq;
using Domain.Core.Commands;

namespace Shell
{
    class Program
    {
        static void Main()
        {
            ConsoleEnvironment.Build();
            PrintHelp();

            while (true)
            {
                Thread.Sleep(300);
                Console.Write("> ");

                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var split = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    HandleRequest(split);
                }
                catch (Exception ex)
                {
                    ConsoleEnvironment.Logger.Fatal(ex.Message);
                }

                Console.WriteLine();
            }
        }

        private static void HandleRequest(string[] split)
        {
            if (ConsoleEnvironment.Commands.ContainsKey(split.First()))
            {
                IConsoleCommand command = ConsoleEnvironment.Commands[split.First()];
                command.Build(split.Skip(1).ToArray());

                ConsoleEnvironment.CommandBus.Submit(command);
            }
            else
            {
                ConsoleEnvironment.Logger.Fatal("Unable to find a matching command");
            }
        }

        private static void PrintHelp()
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Available commands:");

            foreach (var consoleCommand in ConsoleEnvironment.Commands.Values)
            {
                Console.WriteLine(consoleCommand.Usage);
            }

            Console.ForegroundColor = originalColor;
            Console.WriteLine();
        }        
    }
}
