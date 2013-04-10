using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Commands.Commands;

namespace Commands
{
    public class CommandHandler : IHandleCommand<PrintCommand>, IHandleCommand<AddNumbersCommand>
    {
        readonly HashSet<object> commandSpecifications = new HashSet<object>();

        public void AddCommandSpecification<TCommand>(ICommandSpecification<TCommand> specification) where TCommand : ICommand
        {
            commandSpecifications.Add(specification);
        }

        public void Execute(PrintCommand command)
        {
            Validate(command);

            for (int i = 0; i < command.Count; i++)
            {
                Console.WriteLine(command.Text);
            }
        }

        public void Execute(AddNumbersCommand command)
        {
            Validate(command);

            var result = command.X + command.Y;
            Console.WriteLine("{0} = {1} + {2}", result, command.X, command.Y);
        }

        private void Validate<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type specificationType = GetSpecificationType(command);
            IEnumerable<object> specifications = commandSpecifications.Where(specificationType.IsInstanceOfType);
            List<ValidationResult> validationResults = ValidateCommand(command, specifications);

            if (validationResults.Any())
            {
                throw new CommandValidationException(validationResults);
            }
        }

        private static Type GetSpecificationType<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type eventType = command.GetType();
            Type handlerGenericType = typeof(ICommandSpecification<>);
            return handlerGenericType.MakeGenericType(new[] { eventType });
        }

        private static List<ValidationResult> ValidateCommand<TCommand>(TCommand command, IEnumerable<object> specifications) where TCommand : ICommand
        {
            var validationResults = new List<ValidationResult>();

            foreach (object spec in specifications)
            {
                var specification = ((ICommandSpecification<TCommand>)spec);

                if (!specification.IsValid(command))
                {
                    validationResults.Add(new ValidationResult(specification.ErrorMessage));
                }
            }

            return validationResults;
        }        
    }
}