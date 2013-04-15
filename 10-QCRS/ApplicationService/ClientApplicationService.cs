using System;
using Domain.Client.Clients;
using Domain.Client.Clients.Commands;
using Domain.Core.Infrastructure;

namespace ApplicationService
{   
    public class ClientApplicationService : IClientApplicationService
    {
        private readonly IAggregateRepository<Client> clientRepository;
        private readonly IUnitOfWork unitOfWork;

        public ClientApplicationService(IAggregateRepository<Client> clientRepository, IUnitOfWork unitOfWork)
        {
            this.clientRepository = clientRepository;
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

        public void Execute(CorrectDateOfBirth command)
        {
            try
            {
                Client client = clientRepository.Get(command.ClientId);
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
                Client client = clientRepository.Get(command.ClientId);
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
