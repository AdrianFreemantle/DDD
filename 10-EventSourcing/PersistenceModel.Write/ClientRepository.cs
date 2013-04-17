using System;
using System.Collections.Generic;
using System.Linq;

using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Events;
using Domain.Core.Infrastructure;

using EventStore;

namespace PersistenceModel.Write
{
    public sealed class ClientRepository : IClientRepository
    {
        private readonly IStoreEvents eventStore;
        private readonly IPublishEvents eventPublisher;

        public ClientRepository(IStoreEvents eventStore, IPublishEvents eventPublisher)
        {
            this.eventStore = eventStore;
            this.eventPublisher = eventPublisher;
        }

        public Client Get(IHaveIdentity id)
        {
            IEventStream eventStream = eventStore.OpenStream(id.GetSurrogateId());

            if (!eventStream.CommittedEvents.Any())
            {
                throw new KeyNotFoundException("Unable to find any events for the provided aggregate key.");
            }

            var events = eventStream.CommittedEvents.Select(message => (IDomainEvent)message.Body);

            var client = ActivatorHelper.CreateInstance<Client>();
            ((IAggregate)client).LoadFromHistory(events);

            return client;
        }

        public Client Get(IdentityNumber identityNumber)
        {
            return Get(new ClientId(identityNumber));
        }

        public void Save(Client client)
        {
            IEnumerable<IDomainEvent> changes = ((IAggregate)client).GetChanges();
            Guid id = ((IHaveIdentity)client.Identity).GetSurrogateId();
            IEventStream eventStream = eventStore.OpenStream(id);

            foreach (var domainEvent in changes)
            {
                if (eventStream.StreamRevision != (domainEvent.Version - 1))
                {
                    throw new Exception(
                        String.Format("Concurrency violation. Expected stream revision {0}, but actual revision was {1}",
                        (domainEvent.Version - 1), eventStream.StreamRevision));
                }

                eventStream.Add(new EventMessage(domainEvent));
                eventPublisher.Publish(domainEvent);
            }
        }        
    }
}