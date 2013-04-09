using ApplicationService;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.Clients.Services;
using Domain.Core.Commands;
using Domain.Core.Infrastructure;
using Domain.Core.Logging;
using Infrastructure;
using Infrastructure.DomainServices;
using Infrastructure.Repositories;
using PersistenceModel;
using System.Collections.Generic;
using Shell.Commands;

namespace Shell
{
    public class ConsoleEnvironment
    {
        public static IRepository Repository { get; private set; }
        public static IDataQuery DataQuery { get; private set; }
        public static IUnitOfWork UnitOfWork { get; private set; }
        public static IClientProjections ClientProjections { get; private set; }
        public static IAggregateRepository<Client> ClientRepository { get; private set; }
        public static IClientApplicationService ClientApplicationService { get; private set; }
        public static IAccountFactory AccountFactory { get; private set; }
        public static IAccountNumberService AccountNumberService { get; private set; }
        public static IAccountCancellationService AccountCancellationService { get; private set; }
        public static IAccountProjections AccountProjections { get; private set; }
        public static IAggregateRepository<Account> AccountRepository { get; private set; }
        public static IAccountApplicationService AccountApplicationService { get; private set; }
        public static Dictionary<string, IConsoleCommand> Commands { get; private set; }
        public static ICommandPublisher LocalCommandPublisher { get; private set; }

        public static void Build()
        {
            LogFactory.BuildLogger = type => new ConsoleWindowLogger(type);

            Repository = new InMemoryRepository();
            DataQuery = Repository as IDataQuery;
            UnitOfWork = new InMemoryUnitOfWork(Repository);
            ClientProjections = new ClientProjections(Repository);
            ClientRepository = new ClientRepository(Repository);
            ClientApplicationService = new ClientApplicationService(ClientRepository, ClientProjections, UnitOfWork);
            AccountProjections = new AccountProjections(Repository);
            AccountRepository = new AccountRepository(Repository);
            AccountNumberService = new AccountNumberService();
            AccountCancellationService = new AccountCancellationService(DataQuery, AccountRepository);
            AccountFactory = new AccountFactory(AccountNumberService, DataQuery, ClientRepository);
            AccountApplicationService = new AccountApplicationService(AccountRepository, AccountProjections, AccountFactory, AccountCancellationService, UnitOfWork);
            
            RegisterCommands();
            SubscribeToCommands();
        }

        private static void SubscribeToCommands()
        {
            LocalCommandPublisher = new LocalCommandPublisher();

            ((LocalCommandPublisher)LocalCommandPublisher).Subscribe(ClientApplicationService);
            ((LocalCommandPublisher)LocalCommandPublisher).Subscribe(AccountApplicationService);
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
    }
}
