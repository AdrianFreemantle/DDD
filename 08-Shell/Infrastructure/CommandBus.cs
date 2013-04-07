using Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public sealed class CommandBus
    {
        private HashSet<object> handlers;

        public void Submit<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type eventType = command.GetType();
            Type handlerGenericType = typeof(ICommandHandler<>);
            Type handlerType = handlerGenericType.MakeGenericType(new[] { eventType });

            var handler = handlers.Single(a => handlerType.IsAssignableFrom(a.GetType()));

            ((dynamic)handler).Execute((dynamic)command);
        }

        private static Type GetCommandHandlerType<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type eventType = command.GetType();
            Type handlerGenericType = typeof(ICommandHandler<>);
            return handlerGenericType.MakeGenericType(new[] { eventType });
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
