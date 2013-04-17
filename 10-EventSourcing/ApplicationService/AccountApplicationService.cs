using System;
using Domain.Client.Accounts;
using Domain.Client.Accounts.Commands;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;

namespace ApplicationService
{
    public class AccountApplicationService : IAccountApplicationService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IAccountNumberService accountNumberService;
        private readonly IUnitOfWork unitOfWork;

        public AccountApplicationService(IAccountRepository accountRepository, IAccountNumberService accountNumberService, IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.accountNumberService = accountNumberService;
            this.unitOfWork = unitOfWork;
        }

        public void Execute(CancelAccount command)
        {
            try
            {
                Account account = accountRepository.Get(command.AccountNumber);
                account.Cancel();
                accountRepository.Save(account);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Execute(RegisterMissedPayment command)
        {
            try
            {
                Account account = accountRepository.Get(command.AccountNumber);
                account.RegisterPayment(BillingResult.NotPaid(DateTime.Today));
                accountRepository.Save(account);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Execute(RegisterSuccessfullPayment command)
        {
            try
            {
                Account account = accountRepository.Get(command.AccountNumber);
                account.RegisterPayment(command.BillingResult);
                accountRepository.Save(account);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void When(ClientPassedAway @event)
        {
            try
            {
                AccountNumber accountNumber = accountNumberService.GetAccountNumberForClient(@event.ClientId);

                if (accountNumber.IsEmpty())
                {
                    return;
                }
                
                Account account = accountRepository.Get(accountNumber);
                account.Cancel();
                accountRepository.Save(account);
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