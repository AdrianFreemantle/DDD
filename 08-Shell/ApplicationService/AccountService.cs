using System;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;
using ApplicationService.Commands;
using Domain.Core.Commands;

namespace ApplicationService
{
    public class AccountService : 
        ICommandHandler<CancelAccount>,
        ICommandHandler<RegisterMissedPayment>,
        ICommandHandler<RegisterSuccessfullPayment>
    {
        private readonly AggregateRepository<Account> accountRepository;
        private readonly IUnitOfWork unitOfWork;

        public AccountService(AggregateRepository<Account> accountRepository, IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Execute(CancelAccount command)
        {
            try
            {
                Account account = accountRepository.Get(command.AccountNumber.Id);
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
                Account account = accountRepository.Get(command.AccountNumber.Id);
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
                Account account = accountRepository.Get(command.AccountNumber.Id);
                account.RegisterPayment(command.BillingResult);
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