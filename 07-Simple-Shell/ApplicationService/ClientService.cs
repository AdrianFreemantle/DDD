using System;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;
using Domain.Client.Services;
using Domain.Client.Accounts;

namespace ApplicationService
{
    public class ClientService
    {
        private readonly AggregateRepository<Client> clientRepository;
        private readonly IAccountNumberService accountNumberService;
        private readonly IUnitOfWork unitOfWork;

        public ClientService(AggregateRepository<Client> clientRepository, IAccountNumberService accountNumberService, IUnitOfWork unitOfWork)
        {
            this.clientRepository = clientRepository;
            this.accountNumberService = accountNumberService;
            this.unitOfWork = unitOfWork;
        }

        public void RegisterClient(string idNumber, string firstName, string surname, string primaryContactNumber)
        {
            try
            {
                Client.RegisterClient(new IdentityNumber(idNumber), new PersonName(firstName, surname), new TelephoneNumber(primaryContactNumber));
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void OpenAccount(string clientId)
        {
            try
            {
                Client client = clientRepository.Get(clientId);
                client.OpenAccount(accountNumberService);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void CorrectDateOfBirth(string clientId, DateTime dateOfBirth)
        {
            try
            {
                Client client = clientRepository.Get(clientId);
                client.CorrectDateOfBirth(new DateOfBirth(dateOfBirth));
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void ClientIsDeceased(string clientId)
        {
            try
            {
                Client client = clientRepository.Get(clientId);
                client.ClientIsDeceased();
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HandleException(Exception ex)
        {
            unitOfWork.Rollback();
            throw ex;
        }
    }
}
