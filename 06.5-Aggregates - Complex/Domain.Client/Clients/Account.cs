using Domain.Client.Clients.Events;
using Domain.Client.Clients.Snapshots;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{
    public class Account : Entity<AccountNumber>
    {
        private Recency recency;

        protected Account()
        {
        }

        protected Account(AccountNumber accountNumber)
        {
            Identity = accountNumber;
        }

        internal static Account Open(AccountNumber accountNumber)
        {
            var account = new Account();
            account.RaiseEvent(new AccountOpened(accountNumber));
            return account;
        }

        public void When(AccountOpened @event)
        {
            Identity = @event.AccountNumber;
        }

        internal static Account Null()
        {
            return new Account
            {
                Identity = new AccountNumber("00000000")
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (IAccountSnapshot)memento;

            recency = snapshot.Recency;
        }
    }
}