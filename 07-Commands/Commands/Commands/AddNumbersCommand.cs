using System;

namespace Commands.Commands
{
    public class AddNumbersCommand : ICommand
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class AddNumbersConsoleCommand : AddNumbersCommand, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "AddNumbers" }; }
        }

        public string Usage
        {
            get { return "AddNumbers <X> <Y> {Adds integers X and Y}"; }
        }

        public void Build(string[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception(String.Format("Error. Usage is: {0}", Usage));
            }

            X = int.Parse(args[0]);
            Y = int.Parse(args[1]);
        }
    }

    public sealed class SumNotGreaterThanOneThousand : ICommandSpecification<AddNumbersCommand>
    {
        public string ErrorMessage { get { return "The sum of X and Y may not exceed 1000."; } }

        public bool IsValid(AddNumbersCommand command)
        {
            return (command.X + command.Y) <= 1000;
        }
    }

    public sealed class SumNotDivisibleByOneHundred : ICommandSpecification<AddNumbersCommand>
    {
        public string ErrorMessage { get { return "The sum of X and Y may not be divisible by 100."; } }

        public bool IsValid(AddNumbersCommand command)
        {
            return ((command.X + command.Y) % 100) != 0;
        }
    }
}