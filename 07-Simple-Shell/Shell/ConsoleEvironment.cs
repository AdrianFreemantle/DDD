using System;
using ApplicationService;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.Events;
using Domain.Core.Events;
using Domain.Core.Infrastructure;
using Infrastructure;
using Infrastructure.Repositories;
using PersistenceModel;
using Infrastructure.Services;
using ApplicationService.EventHandlers;

namespace Shell
{
    public static class ConsoleEvironment
    {
        private static InMemoryUnitOfWork UnitOfWork;
        private static ClientProjections ClientProjections;
        private static AggregateRepository<Client> ClientRepository;
        private static AccountProjections AccountProjections;
        private static AggregateRepository<Account> AccountRepository;
        private static ClientPassedAwayHandler ClientPassedAwayHandler;

        public static AccountService AccountService { get; private set; }
        public static ClientService ClientService { get; private set; }

        public static void Build()
        {
            UnitOfWork = new InMemoryUnitOfWork();
            ClientProjections = new ClientProjections(UnitOfWork.Repository);
            ClientRepository = new ClientRepository(UnitOfWork.Repository);
            ClientService = new ClientService(ClientRepository, new AccountNumberService(), UnitOfWork);
            AccountProjections = new AccountProjections(UnitOfWork.Repository);
            AccountRepository = new AccountRepository(UnitOfWork.Repository);
            AccountService = new AccountService(AccountRepository, UnitOfWork);
            ClientPassedAwayHandler = new ClientPassedAwayHandler(AccountRepository, UnitOfWork);

            SubscribeToEvents();
        }

        private static void SubscribeToEvents()
        {
            DomainEvent.Current.Subscribe<ClientRegistered>(ClientProjections.When);
            DomainEvent.Current.Subscribe<ClientDateOfBirthCorrected>(ClientProjections.When);
            DomainEvent.Current.Subscribe<ClientPassedAway>(ClientProjections.When);
            DomainEvent.Current.Subscribe<ClientPassedAway>(ClientProjections.When);
            DomainEvent.Current.Subscribe<AccountBilled>(AccountProjections.When);
            DomainEvent.Current.Subscribe<AccountOpened>(AccountProjections.When);
            DomainEvent.Current.Subscribe<AccountOpened>(ClientProjections.When);
            DomainEvent.Current.Subscribe<AccountStatusChanged>(AccountProjections.When);
            DomainEvent.Current.Subscribe<ClientPassedAway>(ClientPassedAwayHandler.When);
        }
    }
}
