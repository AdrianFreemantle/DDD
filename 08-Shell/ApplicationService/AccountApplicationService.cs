using System;
using Domain.Client.Accounts;
using Domain.Client.Accounts.Commands;
using Domain.Client.Accounts.Events;
using Domain.Client.Clients.Events;
using Domain.Client.Clients.Services;
using Domain.Client.ValueObjects;
using Domain.Core.Events;
using Domain.Core.Infrastructure;

namespace ApplicationService
{
    public class AccountApplicationService : IAccountApplicationService
    {
        private readonly IAggregateRepository<Account> accountRepository;
        private readonly IHandleAccountStateTransitions accountProjections;
        private readonly IAccountFactory accountFactory;
        private readonly IAccountCancellationService accountCancellationService;
        private readonly IUnitOfWork unitOfWork;

        public AccountApplicationService(IAggregateRepository<Account> accountRepository, IAccountProjections accountProjections, 
            IAccountFactory accountFactory, IAccountCancellationService accountCancellationService, IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.accountProjections = accountProjections;
            this.accountFactory = accountFactory;
            this.accountCancellationService = accountCancellationService;
            this.unitOfWork = unitOfWork;

            SubsribeToEvents();
        }

        public void Execute(OpenAccount command)
        {
            try
            {
                accountFactory.OpenAccount(command.ClientId);
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
                accountCancellationService.CancelClientAccount(@event.ClientId);
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

        private void SubsribeToEvents()
        {
            DomainEvent.Current.Subscribe<AccountOpened>(accountProjections.When);
            DomainEvent.Current.Subscribe<AccountStatusChanged>(accountProjections.When);
            DomainEvent.Current.Subscribe<AccountBilled>(accountProjections.When);
            DomainEvent.Current.Subscribe<ClientPassedAway>(When);
        }
    }
}