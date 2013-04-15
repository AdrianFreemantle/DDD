using System.Linq;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Infrastructure;

namespace PersistenceModel.Repositories
{
    public sealed class ClientRepository : IClientRepository
    {       
        private readonly IRepository repository;
        private readonly IDataQuery dataQuery;

        public ClientRepository(IRepository repository, IDataQuery dataQuery)
        {
            this.repository = repository;
            this.dataQuery = dataQuery;
        }

        public Client Get<TKey>(IdentityBase<TKey> id)
        {
            var clientModel = repository.Get<ClientModel>(id);
            return BuildClient(clientModel);
        }

        public Client Get(IdentityNumber identityNumber)
        {
            var clientModel = dataQuery
                .GetQueryable<ClientModel>()
                .First(model => model.IdentityNumber == identityNumber.Number);

            return BuildClient(clientModel);
        }

        private Client BuildClient(ClientModel clientModel)
        {
            Mandate.ParameterNotNull(clientModel, "clientModel");

            var client = ActivatorHelper.CreateInstance<Client>();
            (client as IAggregate).RestoreSnapshot(LoadSnapshot(clientModel));
            return client;
        }

        private static IMemento LoadSnapshot(ClientModel clientModel)
        {           
            var snapshot = new ClientSnapshot
            {
                IdentityNumber = new IdentityNumber(clientModel.IdentityNumber),
                Identity = new ClientId(new IdentityNumber(clientModel.IdentityNumber)),
                ClientName = new PersonName(clientModel.FirstName, clientModel.Surname),
                DateOfBirth = new DateOfBirth(clientModel.DateOfBirth),
                IsDeceased = clientModel.IsDeceased,
                PrimaryContactNumber = new TelephoneNumber(clientModel.PrimaryContactNumber),
            };

            return snapshot;
        }

        class ClientSnapshot : IClientSnapshot
        {
            public IHaveIdentity Identity { get; set; }
            public PersonName ClientName { get; set; }
            public TelephoneNumber PrimaryContactNumber { get; set; }
            public DateOfBirth DateOfBirth { get; set; }
            public IdentityNumber IdentityNumber { get; set; }
            public bool IsDeceased { get; set; }
        }        
    }
}