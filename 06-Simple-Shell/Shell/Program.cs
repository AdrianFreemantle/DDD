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
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleEvironment.Build();
                var clientService = ConsoleEvironment.ClientService;

                clientService.RegisterClient("7808035176089", "Adrian", "Freemantle", "0123332435");
                clientService.CorrectDateOfBirth("7808035176089", DateTime.Parse("1980-01-01"));
                clientService.OpenAccount("7808035176089");
                clientService.ClientIsDeceased("7808035176089");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            System.Console.ReadKey();
        }
    }
}
