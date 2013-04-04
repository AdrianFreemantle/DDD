using System.Linq;
using Domain.Client.Events;
using Domain.Core.Events;
using Domain.Core.Infrastructure;

namespace PersistenceModel
{   
    public class ClientProjections : 
        IEventHandler<ClientRegistered>,
        IEventHandler<ClientDateOfBirthCorrected>,
        IEventHandler<ClientPassedAway>
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

        public void When(ClientDateOfBirthCorrected @event)
        {
            var clientModel = FetchClientModel(@event.ClientId.Id);
            clientModel.DateOfBirth = @event.DateOfBirth;
        }

        public void When(ClientPassedAway @event)
        {
            var clientModel = FetchClientModel(@event.ClientId.Id);
            clientModel.IsDeceased = true;
        }

        private ClientModel FetchClientModel(string clientId)
        {
            var clientModel = repository.GetQueryable<ClientModel>().First(client => client.IdentityNumber == clientId);
            return clientModel;
        }
    }
}