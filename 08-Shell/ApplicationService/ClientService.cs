using System;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;
using Domain.Client.Services;
using Domain.Client.Accounts;
using Domain.Core.Commands;
using ApplicationService.Commands;

namespace ApplicationService
{   
    public class ClientService : 
        ICommandHandler<RegisterClient>,
        ICommandHandler<OpenAccount>,
        ICommandHandler<CorrectDateOfBirth>,
        ICommandHandler<SetClientAsDeceased>
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

        public void Execute(RegisterClient command)
        {
            try
            {
                Client.RegisterClient(command.IdentityNumber, command.ClientName, command.PrimaryContactNumber);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Execute(OpenAccount command)
        {
            try
            {
                Client client = clientRepository.Get(command.ClientId.Id);
                client.OpenAccount(accountNumberService);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Execute(CorrectDateOfBirth command)
        {
            try
            {
                Client client = clientRepository.Get(command.ClientId.Id);
                client.CorrectDateOfBirth(command.DateOfBirth);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Execute(SetClientAsDeceased command)
        {
            try
            {
                Client client = clientRepository.Get(command.ClientId.Id);
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
