using System;
using System.Linq;
using System.Threading;

namespace Commands
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
                    Console.WriteLine(ex.Message);
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
                
                ConsoleEnvironment.CommandHandler.Execute((dynamic)command);
            }
            else
            {
                Console.WriteLine("Unable to find a matching command");
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Available commands:");

            foreach (var consoleCommand in ConsoleEnvironment.Commands.Values)
            {
                Console.WriteLine(consoleCommand.Usage);
            }

            Console.WriteLine();
        }
    }
}
