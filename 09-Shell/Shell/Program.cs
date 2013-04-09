using System;
using System.Threading;
using System.Linq;
using Domain.Core.Logging;

namespace Shell
{
    class Program
    {
        static ILog logger;

        static void Main()
        {
            ConsoleEnvironment.Build();
            logger = LogFactory.BuildLogger(typeof(Program));

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
                    logger.Fatal(ex.Message);
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

                ConsoleEnvironment.LocalCommandPublisher.Publish(command);
            }
            else
            {
                logger.Fatal("Unable to find a matching command");
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
