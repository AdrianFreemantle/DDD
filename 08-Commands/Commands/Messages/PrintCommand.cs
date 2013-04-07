using System;

namespace Commands.Messages
{
    public class PrintCommand : ICommand
    {
        public string Text { get; set; }
        public int Count { get; set; }
    }

    public class PrintConsoleCommand : PrintCommand, IConsoleCommand
    {
        public string[] Keys
        {
            get { return new[] { "Print" }; }
        }

        public string Usage
        {
            get { return "Print <text> <count> {Prints the text the specified number of times specified}"; }
        }

        public void Build(string[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception(String.Format("Error. Usage is: {0}", Usage));
            }

            Text = args[0];
            Count = int.Parse(args[1]);
        }
    }
}