using Domain.Client.Accounts.Events;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;

namespace Domain.Client.Accounts
{
    public interface IHandleAccountStateTransitions
    {
        void When(AccountOpened @event);
        void When(AccountStatusChanged @event);
        void When(AccountBilled @event);
    }

    public partial class Account : IHandleAccountStateTransitions
    {
        private AccountStatus accountStatus;
        private Recency recency;
        private ClientId clientId;

        void IHandleAccountStateTransitions.When(AccountOpened @event)
        {
            Identity = @event.AccountNumber;
            clientId = @event.ClientId;
        }

        void IHandleAccountStateTransitions.When(AccountStatusChanged @event)
        {
            accountStatus = @event.Status;
        }

        void IHandleAccountStateTransitions.When(AccountBilled @event)
        {
            recency = @event.Recency;
        }

        protected override void ApplyEvent(object @event)
        {
            ((IHandleAccountStateTransitions)this).When((dynamic)@event);
        }
    }
}
