using Domain.Client.Accounts;
using Domain.Client.Events;
using Domain.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.EventHandlers
{
    public class ClientPassedAwayHandler
    {
        private readonly AggregateRepository<Account> accountRepository;

        public ClientPassedAwayHandler(AggregateRepository<Account> accountRepository, IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
        } 

        public void When(ClientPassedAway @event)
        {
            if (@event.AccountNumber == null)
            {
                return;
            }

            var account = accountRepository.Get(@event.AccountNumber.Id);
            account.Cancel();
        }
    }
}
