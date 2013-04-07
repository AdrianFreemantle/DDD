using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Core.Events
{   
    public sealed class DomainEvent
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static volatile DomainEvent instance;
        private static readonly object SyncRoot = new Object();

        private HashSet<Delegate> actions;
        private IEventBus eventBus;

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

        public void Raise<T>(T @event) where T : IDomainEvent
        {
            if (eventBus != null)
            {
                eventBus.Submit(@event);
            }

            if (actions != null)
            {
                foreach (Action<T> action in actions.OfType<Action<T>>())
                {
                    action(@event);
                }
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

        public void RegisterEventBus(IEventBus bus)
        {
            eventBus = bus;
        }
    }   
}