using System;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Infrastructure;

namespace ApplicationService
{
    public class AccountService
    {
        private readonly AggregateRepository<Account> accountRepository;
        private readonly IUnitOfWork unitOfWork;

        public AccountService(AggregateRepository<Account> accountRepository, IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.unitOfWork = unitOfWork;
        }        

        public void CancelAccount(string accountNumber)
        {
            try
            {
                Account account = accountRepository.Get(accountNumber);
                account.Cancel();
                unitOfWork.Commit();
           
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void RegisterMissedPayment(string accountNumber)
        {
            try
            {
                Account account = accountRepository.Get(accountNumber);
                account.RegisterPayment(BillingResult.NotPaid(DateTime.Today));
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void RegisterSuccessfullPayment(string accountNumber, decimal amount)
        {
            try
            {
                Account account = accountRepository.Get(accountNumber);
                account.RegisterPayment(BillingResult.PaymentMade(amount, DateTime.Today));
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