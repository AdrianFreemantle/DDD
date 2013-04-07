using System;

using Commands.Messages;

namespace Commands.Handlers
{
    public class CommandHandler : IHandleCommand<PrintCommand>, IHandleCommand<AddNumbersCommand>
    {
        public void Execute(PrintCommand command)
        {
            for (int i = 0; i < command.Count; i++)
            {
                Console.WriteLine(command.Text);
            }
        }

        public void Execute(AddNumbersCommand command)
        {
            var result = command.X + command.Y;
            Console.WriteLine("{0} = {1} + {2}", result, command.X, command.Y);
        }
    }
}