using System;
using System.Collections.Generic;
using System.Linq;

using Domain.Client.Accounts;
using Domain.Core;
using Domain.Core.Events;
using Domain.Core.Infrastructure;
using EventStore;

namespace PersistenceModel.Write
{   
    public sealed class AccountRepository : IAccountRepository 
    {
        private readonly IStoreEvents eventStore;
        private readonly IPublishEvents eventPublisher;

        public AccountRepository(IStoreEvents eventStore, IPublishEvents eventPublisher)
        {
            this.eventStore = eventStore;
            this.eventPublisher = eventPublisher;
        }

        public Account Get(IHaveIdentity id)
        {
            IEventStream eventStream = eventStore.OpenStream(id.GetSurrogateId());
            
            if (!eventStream.CommittedEvents.Any())
            {
                throw new InvalidOperationException("Unable to find any events for the provided aggregate key.");
            }

            var events = eventStream.CommittedEvents.Select(message => (IDomainEvent)message.Body);

            var account = ActivatorHelper.CreateInstance<Account>();
            ((IAggregate)account).LoadFromHistory(events);

            return account;
        }

        public void Save(Account account)
        {
            IEnumerable<IDomainEvent> changes = ((IAggregate)account).GetChanges();
            Guid id = ((IHaveIdentity)account.Identity).GetSurrogateId();
            IEventStream eventStream = eventStore.OpenStream(id);

            foreach (var domainEvent in changes)
            {
                if (eventStream.StreamRevision != (domainEvent.Version - 1))
                {
                    throw new Exception(
                        String.Format("Concurrency violation. Expected stream revision {0}, but actual revision was {1}", 
                        domainEvent.Version, eventStream.StreamRevision));
                }

                eventStream.Add(new EventMessage(domainEvent));
                eventPublisher.Publish(domainEvent);
            }
        }
    }
}