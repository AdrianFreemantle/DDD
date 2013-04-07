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
using System.Collections.Generic;
using Shell.Commands;
using ApplicationService.Commands;
using Domain.Core;

namespace Shell
{
    public class ConsoleEnvironment
    {
        private static InMemoryUnitOfWork UnitOfWork;
        private static ClientProjections ClientProjections;
        private static AggregateRepository<Client> ClientRepository;
        private static AccountProjections AccountProjections;
        private static AggregateRepository<Account> AccountRepository;
        private static ClientPassedAwayHandler ClientPassedAwayHandler;

        public static ILog Logger { get; private set; }
        public static AccountService AccountService { get; private set; }
        public static ClientService ClientService { get; private set; }
        public static Dictionary<string, IConsoleCommand> Commands { get; private set; }
        public static CommandBus CommandBus { get; private set; }

        public static void Build()
        {
            Logger = new ConsoleWindowLogger();
            UnitOfWork = new InMemoryUnitOfWork();
            ClientProjections = new ClientProjections(UnitOfWork.Repository, Logger);
            ClientRepository = new ClientRepository(UnitOfWork.Repository);
            ClientService = new ClientService(ClientRepository, new AccountNumberService(), UnitOfWork);
            AccountProjections = new AccountProjections(UnitOfWork.Repository, Logger);
            AccountRepository = new AccountRepository(UnitOfWork.Repository);
            AccountService = new AccountService(AccountRepository, UnitOfWork);
            ClientPassedAwayHandler = new ClientPassedAwayHandler(AccountRepository, UnitOfWork);

            CommandBus = new CommandBus();
            Commands = new Dictionary<string, IConsoleCommand>();
            SubscribeToEvents();
            RegisterCommands();
            SubscribeToCommands();
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

        private static void SubscribeToCommands()
        {
            CommandBus.Subscribe(ClientService);
            CommandBus.Subscribe(AccountService);
        }

        static void RegisterCommands()
        {
            RegisterCommand(new RegisterClientConsoleCommand());
            RegisterCommand(new CorrectDateOfBirthConsoleCommand());
            RegisterCommand(new OpenAccountConsoleCommand());
            RegisterCommand(new SetClientAsDeceasedConsoleCommand());
            RegisterCommand(new CancelAccountConsoleCommand());
            RegisterCommand(new RegisterMissedPaymentConsoleCommand());
            RegisterCommand(new RegisterSuccessfullPaymentConsoleCommand());
        }

        static void RegisterCommand(IConsoleCommand command)
        {
            foreach (var key in command.Keys)
            {
                Commands.Add(key, command);
            }
        }
    }
}
