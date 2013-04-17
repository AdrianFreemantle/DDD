using System;

using Domain.Client.Accounts;
using Domain.Client.Accounts.Commands;
using Domain.Client.Clients;
using Domain.Core.Commands;
using Domain.Core.Infrastructure;

namespace ApplicationService
{
    public class OpenAccountHandler : IHandleCommand<OpenAccount>
    {
        private readonly IClientRepository clientRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IAccountNumberService accountNumberService;
        private readonly IUnitOfWork unitOfWork;

        public OpenAccountHandler(IClientRepository clientRepository, IAccountRepository accountRepository, IAccountNumberService accountNumberService, IUnitOfWork unitOfWork)
        {
            this.clientRepository = clientRepository;
            this.accountRepository = accountRepository;
            this.accountNumberService = accountNumberService;
            this.unitOfWork = unitOfWork;
        }

        public void Execute(OpenAccount command)
        {
            try
            {
                var client = clientRepository.Get(command.ClientId);
                var account = client.OpenAccount(accountNumberService);
                clientRepository.Save(client);
                accountRepository.Save(account);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }
    }
}