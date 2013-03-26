using System;
using System.Collections.Generic;
using System.Linq;

using Domain.Core.Ioc;

namespace Domain.Core.Events
{
    public sealed class DomainEvent
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static volatile DomainEvent instance;
        private static List<Delegate> actions;
        private static readonly object SyncRoot = new Object();

        private readonly IEventBus eventBus;

        private DomainEvent()
        {
            try
            {
                eventBus = ServiceLocator.Current.GetService<IEventBus>();
            }
            catch
            {
                eventBus = null;
            }
        }

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

        public void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
            {
                actions = new List<Delegate>();
            }

            actions.Add(callback);
        }

        public void ClearCallbacks()
        {
            actions = null;
        }
    }   
}