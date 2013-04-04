using System;
using ApplicationService;
using Domain.Client.Clients;
using Domain.Client.Events;
using Domain.Core.Events;
using Domain.Core.Infrastructure;
using Infrastructure;
using Infrastructure.Repositories;
using PersistenceModel;

namespace Console
{
    class Program
    {
        private static readonly InMemoryUnitOfWork UnitOfWork = new InMemoryUnitOfWork();
        private static readonly ClientProjections ClientProjections = new ClientProjections(UnitOfWork.Repository);
        private static readonly AggregateRepository<Client> ClientRepository = new ClientRepository(UnitOfWork.Repository);
        private static readonly ClientService ClientService = new ClientService(ClientRepository, UnitOfWork);

        static void Main(string[] args)
        {
            try
            {
                SubscribeToEvents();
                ClientService.RegisterClient("7808035176089", "Adrian", "Freemantle", "0123332435");
                ClientService.CorrectDateOfBirth("7808035176089", DateTime.Parse("1980-01-01"));
                ClientService.ClientIsDeceased("7808035176089");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            System.Console.ReadKey();
        }

        private static void SubscribeToEvents()
        {
            DomainEvent.Current.Subscribe<ClientRegistered>(ClientProjections.When);
            DomainEvent.Current.Subscribe<ClientDateOfBirthCorrected>(ClientProjections.When);
            DomainEvent.Current.Subscribe<ClientPassedAway>(ClientProjections.When);
        }
    }
}
