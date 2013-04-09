using Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public sealed class LocalCommandPublisher : ICommandPublisher
    {
        private HashSet<object> handlers;

        public void Publish<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handlerType = GetHandlerType(command);
            object handler = handlers.Single(handlerType.IsInstanceOfType);

            ((dynamic)handler).Execute((dynamic)command);
        }

        private static Type GetHandlerType<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type eventType = command.GetType();
            Type handlerGenericType = typeof (IHandleCommand<>);
            return handlerGenericType.MakeGenericType(new[] {eventType});
        }

        public void Subscribe(object handler)
        {
            if (handlers == null)
            {
                handlers = new HashSet<object>();
            }

            handlers.Add(handler);
        }
    }
}
