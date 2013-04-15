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
        private readonly IAggregateRepository<Account> accountRepository;
        private readonly IAccountNumberService accountNumberService;
        private readonly IUnitOfWork unitOfWork;

        public AccountApplicationService(IAggregateRepository<Account> accountRepository, IAccountNumberService accountNumberService, IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.accountNumberService = accountNumberService;
            this.unitOfWork = unitOfWork;
        }

        public void Execute(OpenAccount command)
        {
            try
            {
                Account.Open(command.ClientId, accountNumberService.GetNextAccountNumber());
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Execute(CancelAccount command)
        {
            try
            {
                Account account = accountRepository.Get(command.AccountNumber);
                account.Cancel();
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