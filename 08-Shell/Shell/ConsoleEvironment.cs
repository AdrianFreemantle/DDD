﻿using ApplicationService;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.Validators;
using Domain.Core.Commands;
using Domain.Core.Infrastructure;
using Domain.Core.Logging;
using Services;
using Infrastructure;
using PersistenceModel;
using System.Collections.Generic;
using PersistenceModel.Repositories;
using Shell.ConsoleCommands;

namespace Shell
{
    public class ConsoleEnvironment
    {
        public static IRepository Repository { get; private set; }
        public static IDataQuery DataQuery { get; private set; }
        public static IUnitOfWork UnitOfWork { get; private set; }
        public static IClientProjections ClientProjections { get; private set; }
        public static IClientRepository ClientRepository { get; private set; }
        public static IClientApplicationService ClientApplicationService { get; private set; }
        public static IAccountNumberService AccountNumberService { get; private set; }
        public static IAccountProjections AccountProjections { get; private set; }
        public static IAccountRepository AccountRepository { get; private set; }
        public static IAccountApplicationService AccountApplicationService { get; private set; }
        public static Dictionary<string, IConsoleCommand> Commands { get; private set; }
        public static IPublishCommands LocalCommandPublisher { get; private set; }

        public static void Build()
        {
            LogFactory.BuildLogger = type => new ConsoleWindowLogger(type);

            Repository = new InMemoryRepository();
            DataQuery = Repository as IDataQuery;
            UnitOfWork = new InMemoryUnitOfWork(Repository);
            ClientProjections = new ClientProjections(Repository);
            ClientRepository = new ClientRepository(Repository, DataQuery);
            ClientApplicationService = new ClientApplicationService(ClientRepository, ClientProjections, UnitOfWork);
            AccountProjections = new AccountProjections(Repository);
            AccountRepository = new AccountRepository(Repository);
            AccountNumberService = new AccountNumberService(DataQuery);
            AccountApplicationService = new AccountApplicationService(AccountRepository, AccountProjections, AccountNumberService, UnitOfWork);
            
            RegisterCommands();
            SubscribeToCommands();
            RegisterSpecifications();
        }

        static void RegisterCommands()
        {
            Commands = new Dictionary<string, IConsoleCommand>();

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

        private static void SubscribeToCommands()
        {
            LocalCommandPublisher = new LocalCommandPublisher();

            ((LocalCommandPublisher)LocalCommandPublisher).Subscribe(ClientApplicationService);
            ((LocalCommandPublisher)LocalCommandPublisher).Subscribe(AccountApplicationService);
        }

        static void RegisterSpecifications()
        {
            ((LocalCommandPublisher)LocalCommandPublisher).RegisterSpecification(new RegisterClientValidator(ClientRepository));
            ((LocalCommandPublisher)LocalCommandPublisher).RegisterSpecification(new OpenAccountValidator(ClientRepository, AccountNumberService));
        }
    }
}
