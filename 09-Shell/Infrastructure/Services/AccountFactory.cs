using System.Linq;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.Clients.Services;
using Domain.Core;
using Domain.Core.Infrastructure;
using PersistenceModel;

namespace Infrastructure.Services
{
    public sealed class AccountFactory : IAccountFactory
    {
        private readonly IAccountNumberService accountNumberService;
        private readonly IDataQuery dataQuery;
        private readonly IAggregateRepository<Client> clientRepository;

        public AccountFactory(IAccountNumberService accountNumberService, IDataQuery dataQuery, IAggregateRepository<Client> clientRepository)
        {
            this.accountNumberService = accountNumberService;
            this.dataQuery = dataQuery;
            this.clientRepository = clientRepository;
        }

        public Account OpenAccount(ClientId clientId)
        {
            if (ClientHasAccount(clientId))
            {
                throw DomainError.Named("account-exists", "The client already has an account.");
            }

            Client client = clientRepository.Get(clientId);

            if (!client.ClientMayOpenAccount())
            {
                throw DomainError.Named("new-account-not-allowed", "The client is not currently able to open an account");
            }

            return Account.Open(clientId, accountNumberService.GetNextAccountNumber());
        }

        bool ClientHasAccount(ClientId clientId)
        {
            return dataQuery.GetQueryable<AccountModel>().Any(account => account.ClientId == clientId.Id);
        }
    }
}