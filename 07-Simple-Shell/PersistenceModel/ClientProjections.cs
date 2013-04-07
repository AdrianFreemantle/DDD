using System.Linq;
using Domain.Client.Clients;
using Domain.Client.Events;
using Domain.Core.Infrastructure;

namespace PersistenceModel
{
    public class ClientProjections : IHandleClientStateTransitions
    {
        private readonly IRepository repository;

        public ClientProjections(IRepository repository)
        {
            this.repository = repository;
        }

        public void When(ClientRegistered @event)
        {
            var clientModel = new ClientModel
            {
                DateOfBirth = @event.IdentityNumber.GetDateOfBirth(),
                IdentityNumber = @event.IdentityNumber.Number,
                FirstName = @event.ClientName.FirstName,
                Surname = @event.ClientName.Surname,
                PrimaryContactNumber = @event.PrimaryContactNumber.Number,
                IsDeceased = false
            };

            repository.Add(clientModel);
        }

        public void When(AccountOpened @event)
        {
            var clientModel = FetchModel(@event.ClientId);
            clientModel.AccountNumber = @event.AccountNumber.Id;
        }

        public void When(ClientDateOfBirthCorrected @event)
        {
            var clientModel = FetchModel(@event.ClientId);
            clientModel.DateOfBirth = @event.DateOfBirth;
        }

        public void When(ClientPassedAway @event)
        {
            var clientModel = FetchModel(@event.ClientId);
            clientModel.IsDeceased = true;
        }

        private ClientModel FetchModel(ClientId clientId)
        {
            return repository.GetQueryable<ClientModel>().First(client => client.IdentityNumber == clientId.Id);
        }
    }
}