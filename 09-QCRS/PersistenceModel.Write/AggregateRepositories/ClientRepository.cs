using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Infrastructure;

namespace PersistenceModel.Write.AggregateRepositories
{
    public sealed class ClientRepository : IClientRepository
    {
        private readonly IDocumentStore documentStore;

        public ClientRepository(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public Client Get(IdentityNumber identityNumber)
        {
            return Get(new ClientId(identityNumber));
        }

        public Client Get(IHaveIdentity id)
        {
            var snapshot = documentStore.Get<ClientSnapshot>(id.ToString());
            var client = ActivatorHelper.CreateInstance<Client>();
            ((IAggregate)client).RestoreSnapshot(snapshot);
            return client;
        }

        public void Save(Client client)
        {
            IMemento memento = ((IAggregate)client).GetSnapshot();
            documentStore.Save(memento.Identity.ToString(), memento);
        }
    }
}