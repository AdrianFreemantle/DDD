using Domain.Client.Accounts.Events;
using Domain.Client.Clients;
using Domain.Client.Clients.Events;
using Domain.Core.Infrastructure;
using Domain.Core.Logging;

namespace PersistenceModel
{   
    public class ClientProjections : IClientProjections
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(ClientProjections));
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
            Logger.Verbose(@event.ToString());
        }

        public void When(AccountOpened @event)
        {
            var clientModel = FetchModel(@event.ClientId);
            clientModel.AccountNumber = @event.AccountNumber.Id;
            Logger.Verbose(@event.ToString());
        }

        public void When(ClientDateOfBirthCorrected @event)
        {
            var clientModel = FetchModel(@event.ClientId);
            clientModel.DateOfBirth = @event.DateOfBirth;
            Logger.Verbose(@event.ToString());
        }

        public void When(ClientPassedAway @event)
        {
            var clientModel = FetchModel(@event.ClientId);
            clientModel.IsDeceased = true;
            Logger.Verbose(@event.ToString());
        }

        private ClientModel FetchModel(ClientId clientId)
        {
            return repository.Get<ClientModel>(clientId.Id);
        }
    }
}