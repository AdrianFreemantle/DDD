using System;
using System.Collections.Generic;
using System.Linq;

using Domain.Core.Events;
using Domain.Core.Logging;

namespace Infrastructure
{
    public sealed class LocalEventPublisher : IPublishEvents
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(LocalCommandPublisher));

        private readonly HashSet<object> registeredHandlers;

        public LocalEventPublisher()
        {
            registeredHandlers = new HashSet<object>();
        }

        public void Subscribe(object handler)
        {
            registeredHandlers.Add(handler);
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            Type handlerGenericType = typeof(IHandleEvent<>);
            Type handlerType = handlerGenericType.MakeGenericType(new[] { @event.GetType() });
            IEnumerable<object> handlers = registeredHandlers.Where(handlerType.IsInstanceOfType);

            Logger.Verbose(@event.ToString());

            foreach (var handler in handlers)
            {
                ((dynamic)handler).When((dynamic)@event);
            }
        }
    }
}