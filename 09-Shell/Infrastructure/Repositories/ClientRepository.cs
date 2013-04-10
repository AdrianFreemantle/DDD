using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Infrastructure;
using PersistenceModel;

namespace Infrastructure.Repositories
{
    public sealed class ClientRepository : AggregateRepository<Client>
    {       
        private readonly IRepository repository;

        public ClientRepository(IRepository repository)
        {
            this.repository = repository;
        }

        protected override IMemento LoadSnapshot(object id)
        {
            var clientModel = repository.Get<ClientModel>(id);

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