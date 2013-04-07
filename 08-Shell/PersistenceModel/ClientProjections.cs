using System.Linq;
using Domain.Client.Clients;
using Domain.Client.Events;
using Domain.Core.Infrastructure;
using Domain.Core;

namespace PersistenceModel
{
    public class ClientProjections : IHandleClientStateTransitions
    {
        private readonly IRepository repository;
        private readonly ILog logger;

        public ClientProjections(IRepository repository, ILog logger)
        {
            this.repository = repository;
            this.logger = logger;
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

            logger.Verbose("Registered new client {0} {1}.", @event.ClientName.FirstName, @event.ClientName.Surname);
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

            logger.Verbose("Update date of birth for client {0} to {1}.", @event.ClientId.Id, @event.DateOfBirth.Date.ToShortDateString());
        }

        public void When(ClientPassedAway @event)
        {
            var clientModel = FetchModel(@event.ClientId);
            clientModel.IsDeceased = true;

            logger.Verbose("Client {0} has passed away. They will be missed.", @event.ClientId.Id);
        }

        private ClientModel FetchModel(ClientId clientId)
        {
            return repository.GetQueryable<ClientModel>().First(client => client.IdentityNumber == clientId.Id);
        }
    }
}