using System.ComponentModel.DataAnnotations;
using Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    /// <summary>
    /// This publisher will publish the command messages to local subscribers, this means we are not crossing process boundaries.
    /// It is however possible to have a command publisher that sends our command messages to a remote process via a 
    /// messaging technology such as WCF, Rabbit MQ, Zer0 MQ, MSMQ, Serivce Bus, nServiceBus etc. This meas we can easily combine 
    /// or separate our web and worker roles into a single or distributed process.
    /// </summary>
    public sealed class LocalCommandPublisher : IPublishCommands
    {
        private readonly HashSet<object> handlers;
        private readonly HashSet<object> commandSpecifications;

        public LocalCommandPublisher()
        {
            handlers = new HashSet<object>();
            commandSpecifications = new HashSet<object>();
        }

        public void Subscribe(object handler)
        {
            handlers.Add(handler);
        }

        public void RegisterSpecification<TCommand>(IValidateCommand<TCommand> specification) where TCommand : ICommand
        {
            commandSpecifications.Add(specification);
        }

        public void Publish<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type handlerGenericType = typeof(IHandleCommand<>);
            Type handlerType = handlerGenericType.MakeGenericType(new[] { command.GetType() });
            object handler = handlers.Single(handlerType.IsInstanceOfType);

            Validate(command);

            ((dynamic)handler).Execute((dynamic)command);
        }

        private void Validate<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type validatorGenericType = typeof(IValidateCommand<>);
            Type specificationType = validatorGenericType.MakeGenericType(new[] { command.GetType() });
            object validator = commandSpecifications.SingleOrDefault(specificationType.IsInstanceOfType);

            if (validator != null)
            {
                ValidationResult[] validationResults = ((dynamic)validator).Validate((dynamic)command);

                if (validationResults.Any())
                {
                    throw new CommandValidationException(validationResults);
                }
            }
        }
    }
}
