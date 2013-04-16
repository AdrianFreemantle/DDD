using Domain.Client.Clients;
using Domain.Client.Events;
using Domain.Client.ValueObjects;

namespace Domain.Client.Accounts
{
    interface IAccountState
    {
        void When(AccountOpened @event);
        void When(AccountStatusChanged @event);
        void When(AccountBilled @event);
    }

    public partial class Account : IAccountState
    {
        private AccountStatus accountStatus;
        private Recency recency;
        private ClientId clientId;

        void IAccountState.When(AccountOpened @event)
        {
            Identity = @event.AccountNumber;
            clientId = @event.ClientId;
        }

        void IAccountState.When(AccountStatusChanged @event)
        {
            accountStatus = @event.Status;
        }

        void IAccountState.When(AccountBilled @event)
        {
            recency = @event.Recency;
        }

        protected override void ApplyEvent(object @event)
        {
            ((IAccountState)this).When((dynamic)@event);
        }
    }
}
