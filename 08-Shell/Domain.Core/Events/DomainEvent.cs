using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Core.Events
{
    public sealed class DomainEvent : IEventPublisher
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static volatile DomainEvent instance;
        private static readonly object SyncRoot = new Object();

        private HashSet<Delegate> actions;
        private IEventPublisher eventPublisher;

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

        public void RegisterEventBus(IEventPublisher publisher)
        {
            eventPublisher = publisher;
        }
    }   
}