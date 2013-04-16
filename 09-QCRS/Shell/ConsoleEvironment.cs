using ApplicationService;
using Domain.Client.Accounts;
using Domain.Client.Accounts.Events;
using Domain.Client.Clients;
using Domain.Client.Clients.Events;
using Domain.Client.Validators;
using Domain.Client.ValueObjects;
using Domain.Core.Commands;
using Domain.Core.Events;
using Domain.Core.Infrastructure;
using Domain.Core.Logging;
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
        public static IDocumentStore DocumentStore { get; private set; }
        public static IRepository Repository { get; private set; }
        public static IDataQuery DataQuery { get; private set; }
        public static IUnitOfWork UnitOfWork { get; private set; }
        public static IClientRepository ClientRepository { get; private set; }
        public static IClientApplicationService ClientApplicationService { get; private set; }
        public static IAccountNumberService AccountNumberService { get; private set; }
        public static IAccountRepository AccountRepository { get; private set; }
        public static IAccountApplicationService AccountApplicationService { get; private set; }
        public static Dictionary<string, IConsoleCommand> Commands { get; private set; }
        public static Dictionary<string, IConsoleView> Views { get; private set; }
        public static IPublishCommands LocalCommandPublisher { get; private set; }

        public static AccountStatusHistoryProjection AccountStatusHistoryProjection { get; private set; }
        public static ClientViewProjections ClientViewProjections { get; private set; }

        public static void Build()
        {
            LogFactory.BuildLogger = type => new ConsoleWindowLogger(type);

            Repository = new InMemoryRepository();
            DocumentStore = new InMemeoryDocumentStore();
            DataQuery = Repository as IDataQuery;
            UnitOfWork = new InMemoryUnitOfWork(Repository);
          
            AccountStatusHistoryProjection = new AccountStatusHistoryProjection(Repository);
            ClientViewProjections = new ClientViewProjections(Repository);

            ClientRepository = new ClientRepository(DocumentStore);
            ClientApplicationService = new ClientApplicationService(ClientRepository, UnitOfWork);
            AccountRepository = new AccountRepository(DocumentStore);
            AccountNumberService = new AccountNumberService(DataQuery);
            AccountApplicationService = new AccountApplicationService(AccountRepository, AccountNumberService, UnitOfWork);

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

        static void RegisterCommandValidators()
        {
            ((LocalCommandPublisher)LocalCommandPublisher).RegisterSpecification(new RegisterClientValidator(ClientRepository));
            ((LocalCommandPublisher)LocalCommandPublisher).RegisterSpecification(new OpenAccountValidator(ClientRepository, AccountNumberService));
        }

        static void SubsribeToEvents()
        {
            DomainEvent.Current.Subscribe<ClientPassedAway>(AccountApplicationService.When);

            DomainEvent.Current.Subscribe<AccountOpened>(ClientViewProjections.When);
            DomainEvent.Current.Subscribe<AccountStatusChanged>(ClientViewProjections.When);
            DomainEvent.Current.Subscribe<AccountBilled>(ClientViewProjections.When);
            DomainEvent.Current.Subscribe<ClientPassedAway>(ClientViewProjections.When);
            DomainEvent.Current.Subscribe<ClientRegistered>(ClientViewProjections.When);
            DomainEvent.Current.Subscribe<ClientDateOfBirthCorrected>(ClientViewProjections.When);
            DomainEvent.Current.Subscribe<ClientPassedAway>(ClientViewProjections.When);

            DomainEvent.Current.Subscribe<AccountStatusChanged>(AccountStatusHistoryProjection.When);
        }
    }
}
