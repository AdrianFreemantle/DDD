using System;
using System.Threading;
using System.Linq;
using Domain.Core.Commands;
using Domain.Core.Logging;

namespace Shell
{
    class Program
    {
        static ILog logger;

        static void Main()
        {
            Console.WindowHeight = 40;
            Console.WindowWidth = 120;

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

                var split = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                TryHandleRequest(split);

                Console.WriteLine();
            }
        }

        private static void TryHandleRequest(string[] split)
        {
            try
            {
                HandleRequest(split);
            }
            catch (CommandValidationException ex)
            {
                foreach (var validationResult in ex.ValidationResults)
                {
                    logger.Fatal(validationResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message);
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
            else if (ConsoleEnvironment.Views.ContainsKey(split.First()))
            {
                IConsoleView view = ConsoleEnvironment.Views[split.First()];
                view.Print(split.Skip(1).ToArray());
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

            Console.WriteLine();
            Console.WriteLine("Available views:");

            foreach (var consoleView in ConsoleEnvironment.Views.Values)
            {
                Console.WriteLine(consoleView.Usage);
            }

            Console.ForegroundColor = originalColor;
            Console.WriteLine();
            Console.WriteLine();
        }        
    }
}
