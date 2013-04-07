using System.Linq;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Domain.Core.Infrastructure;
using PersistenceModel;

namespace Infrastructure.Repositories
{
    public class ClientRepository : AggregateRepository<Client>
    {       
        private readonly IRepository repository;

        public ClientRepository(IRepository repository)
        {
            this.repository = repository;
        }

        protected override IMemento LoadSnapshot(object id)
        {
            var clientModel = repository.GetQueryable<ClientModel>()
                                        .First(client => client.IdentityNumber == id.ToString());

            var snapshot = new ClientSnapshot
            {
                IdentityNumber = new IdentityNumber(clientModel.IdentityNumber),
                Identity = new ClientId(new IdentityNumber(clientModel.IdentityNumber)),
                ClientName = new PersonName(clientModel.FirstName, clientModel.Surname),
                DateOfBirth = new DateOfBirth(clientModel.DateOfBirth),
                IsDeceased = clientModel.IsDeceased,
                PrimaryContactNumber = new TelephoneNumber(clientModel.PrimaryContactNumber),
            };

            if(!string.IsNullOrWhiteSpace(clientModel.AccountNumber))
            {
                snapshot.AccountNumber = new Domain.Client.Accounts.AccountNumber(clientModel.AccountNumber);
            }

            return snapshot;
        }
    }
}