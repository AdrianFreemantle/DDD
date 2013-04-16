using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Core.Logging;

namespace Domain.Core.Events
{
    public sealed class DomainEvent : IPublishEvents
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static volatile DomainEvent instance;
        private static readonly object SyncRoot = new Object();
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(DomainEvent));

        private HashSet<Delegate> actions;
        private IPublishEvents eventPublisher;

        private DomainEvent() { }

        public static DomainEvent Current
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                            instance = new DomainEvent();
                    }
                }

                return instance;
            }
        }

        public void Publish<T>(T @event) where T : IDomainEvent
        {           
            if (actions != null)
            {
                foreach (Action<T> action in actions.OfType<Action<T>>())
                {
                    action(@event);
                }
            }

            if (eventPublisher != null)
            {
                eventPublisher.Publish(@event);
            }

            Logger.Verbose(@event.ToString());
        }

        public void Subscribe<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
            {
                actions = new HashSet<Delegate>();
            }

            actions.Add(callback);
        }

        public void ClearSubscribers()
        {
            actions = null;
        }

        public void RegisterEventBus(IPublishEvents publisher)
        {
            eventPublisher = publisher;
        }
    }   
}