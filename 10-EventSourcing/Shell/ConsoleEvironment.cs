using ApplicationService;
using Domain.Client.Accounts;
using Domain.Client.Accounts.Commands;
using Domain.Client.Clients;
using Domain.Client.Validators;
using Domain.Client.ValueObjects;
using Domain.Core.Commands;
using Domain.Core.Infrastructure;
using Domain.Core.Logging;

using EventStore;

using PersistenceModel.Reporting;
using PersistenceModel.Reporting.Projections;
using PersistenceModel.Write;
using Queries;
using Services;
using Infrastructure;
using System.Collections.Generic;
using Shell.ConsoleCommands;
using Shell.ConsoleViews;

namespace Shell
{
    public class ConsoleEnvironment
    {
        public static IStoreEvents EventStore { get; private set; }
        public static IRepository Repository { get; private set; }
        public static IDataQuery DataQuery { get; private set; }
        public static IUnitOfWork UnitOfWork { get; private set; }
        public static IClientRepository ClientRepository { get; private set; }
        public static IClientApplicationService ClientApplicationService { get; private set; }
        public static IAccountNumberService AccountNumberService { get; private set; }
        public static IAccountRepository AccountRepository { get; private set; }
        public static IHandleCommand<OpenAccount> OpenAccountHandler { get; private set; }
        public static IAccountApplicationService AccountApplicationService { get; private set; }
        public static Dictionary<string, IConsoleCommand> Commands { get; private set; }
        public static Dictionary<string, IConsoleView> Views { get; private set; }
        public static LocalCommandPublisher LocalCommandPublisher { get; private set; }
        public static LocalEventPublisher LocalEventPublisher { get; private set; }

        public static ClientLoyaltyCardProjections ClientLoyaltyCardProjections { get; private set; }
        public static AccountStatusHistoryProjection AccountStatusHistoryProjection { get; private set; }
        public static ClientViewProjections ClientViewProjections { get; private set; }

        public static void Build()
        {
            LogFactory.BuildLogger = type => new ConsoleWindowLogger(type);
            
            Repository = new InMemoryRepository();
            EventStore = new InMemoryEventStore();
            DataQuery = Repository as IDataQuery;
            UnitOfWork = new InMemoryUnitOfWork(Repository);
            LocalEventPublisher = new LocalEventPublisher();
            LocalCommandPublisher = new LocalCommandPublisher();

            ClientLoyaltyCardProjections = new ClientLoyaltyCardProjections(Repository);
            AccountStatusHistoryProjection = new AccountStatusHistoryProjection(Repository);
            ClientViewProjections = new ClientViewProjections(Repository);

            ClientRepository = new ClientRepository(EventStore, LocalEventPublisher);
            ClientApplicationService = new ClientApplicationService(ClientRepository, UnitOfWork);
            AccountRepository = new AccountRepository(EventStore, LocalEventPublisher);
            AccountNumberService = new AccountNumberService(DataQuery);
            AccountApplicationService = new AccountApplicationService(AccountRepository, AccountNumberService, UnitOfWork);
            OpenAccountHandler = new OpenAccountHandler(ClientRepository, AccountRepository, AccountNumberService, UnitOfWork);

            RegisterCommands();
            RegisterViews();
            SubscribeToCommands();
            RegisterCommandValidators();
            SubsribeToEvents();

            LookupTables.Register<AccountStatusLookup, AccountStatusType>(Repository);
        }

        static void RegisterViews()
        {
            Views = new Dictionary<string, IConsoleView>();
            RegisterView(new AllClientsConsoleView(new ClientQueries(DataQuery)));
            RegisterView(new AccountStatusHistoryConsoleView(new AccountQueries(DataQuery)));
            RegisterView(new StolenCardsConsoleView(new LoyaltyCardQueries(DataQuery)));
            RegisterView(new ClientCardsConsoleView(new LoyaltyCardQueries(DataQuery)));
            RegisterView(new CancelledCardsConsoleView(new LoyaltyCardQueries(DataQuery)));
        }

        static void RegisterView(IConsoleView view)
        {
            Views.Add(view.Key, view);
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
            RegisterCommand(new CancelLoyaltyCardConsoleCommand());
            RegisterCommand(new ReportLoyaltyCardAsStolenConsoleCommand());
            RegisterCommand(new IssueLoyaltyCardConsoleCommand());
        }

        static void RegisterCommand(IConsoleCommand command)
        {
            foreach (var key in command.Keys)
            {
                Commands.Add(key, command);
            }
        }

        private static void SubsribeToEvents()
        {
            LocalEventPublisher.Subscribe(ClientViewProjections);
            LocalEventPublisher.Subscribe(AccountStatusHistoryProjection);
            LocalEventPublisher.Subscribe(AccountApplicationService);
            LocalEventPublisher.Subscribe(ClientLoyaltyCardProjections);
        }

        private static void SubscribeToCommands()
        {
            LocalCommandPublisher.Subscribe(OpenAccountHandler);
            LocalCommandPublisher.Subscribe(ClientApplicationService);
            LocalCommandPublisher.Subscribe(AccountApplicationService);
        }

        static void RegisterCommandValidators()
        {
            LocalCommandPublisher.RegisterValidator(new IssueLoyaltyCardValidator(AccountRepository, ClientRepository));
            LocalCommandPublisher.RegisterValidator(new RegisterClientValidator(ClientRepository));
            LocalCommandPublisher.RegisterValidator(new OpenAccountValidator(ClientRepository, AccountNumberService));
        }
    }
}
