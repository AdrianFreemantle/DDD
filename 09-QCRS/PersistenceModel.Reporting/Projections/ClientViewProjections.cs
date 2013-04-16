using Domain.Client.Accounts;
using Domain.Client.Accounts.Events;
using Domain.Client.Clients;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;

namespace PersistenceModel.Reporting.Projections
{
    public class ClientViewProjections : IAccountProjections, IClientProjections
    {
        private readonly IRepository repository;

        public ClientViewProjections(IRepository repository)
        {
            this.repository = repository;
        }

        public void When(AccountOpened @event)
        {
            var clientView = FetchClientView(@event.ClientId);
            var accountStatusLookup = repository.Get<AccountStatusLookup>((int)AccountStatusType.Active);

            clientView.AccountNumber = @event.AccountNumber.Id;
            clientView.AccountStatus = accountStatusLookup;
            clientView.AccountRecency = 0;
        }        

        public void When(AccountStatusChanged @event)
        {
            var clientView = FetchClientView(@event.ClientId);
            var accountStatusLookup = repository.Get<AccountStatusLookup>((int)@event.Status.Status);
            clientView.AccountStatus = accountStatusLookup;
        }

        public void When(AccountBilled @event)
        {
            var clientView = FetchClientView(@event.ClientId);
            clientView.AccountRecency = @event.Recency.Value;
        }

        public void When(ClientRegistered @event)
        {
            var accountStatus = repository.Get<AccountStatusLookup>((int)AccountStatusType.Unknown);

            var clientView = new ClientView
            {
                AccountNumber = string.Empty,
                AccountRecency = 0,
                AccountStatus = accountStatus,
                IsDeceased = false,
                DateOfBirth = @event.IdentityNumber.GetDateOfBirth(),
                FirstName = @event.ClientName.FirstName,
                Surname = @event.ClientName.Surname,
                IdentityNumber = @event.IdentityNumber.Number,
                PrimaryContactNumber = @event.PrimaryContactNumber.Number
            };

            repository.Add(clientView);
        }

        public void When(ClientDateOfBirthCorrected @event)
        {
            var clientView = FetchClientView(@event.ClientId);
            clientView.DateOfBirth = @event.DateOfBirth;
        }

        public void When(ClientPassedAway @event)
        {
            var clientView = FetchClientView(@event.ClientId);
            clientView.IsDeceased = true;
        }

        private ClientView FetchClientView(ClientId clientId)
        {
            return repository.Get<ClientView>(clientId.Id);
        }
    }
}