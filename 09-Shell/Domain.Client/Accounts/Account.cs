using Domain.Client.Accounts.Events;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Accounts
{
    public partial class Account : Aggregate<AccountNumber>
    {       
        protected Account()
        {
            recency = Recency.UpToDate();
            accountStatus = new AccountStatus(AccountStatusType.Active);
        }

        public static Account Open(ClientId clientId, AccountNumber accountNumber)
        {
            Mandate.ParameterNotNull(accountNumber, "accountNumber");
            Mandate.ParameterNotNull(clientId, "clientId");

            var account = new Account();
            account.RaiseEvent(new AccountOpened(clientId, accountNumber));
            return account;
        }

        public void Cancel()
        {
            SetAccountStatus(AccountStatusType.Cancelled);
        }

        private void SetAccountStatus(AccountStatusType status)
        {
            if (!accountStatus.StatusMayBeChanged())
            {
                throw DomainError.Named("invalid-status-chage", "The account statatus may not be updated to {0} from {1}.", accountStatus.Status, status);
            }

            RaiseEvent(new AccountStatusChanged(Identity, new AccountStatus(status)));
        }

        public void RegisterPayment(BillingResult billingResult)
        {
            Mandate.ParameterNotNull(billingResult, "billingResult");

            RaiseEvent(new AccountBilled(Identity, recency.FromBillingResult(billingResult)));
            UpdateStatusBasedOnRecency();
        }

        private void UpdateStatusBasedOnRecency()
        {
            if (recency.IsUpToDate())
            {
                SetAccountStatus(AccountStatusType.Active);
            }
            else if (recency.ShouldAccountBeSuspended())
            {
                SetAccountStatus(AccountStatusType.Suspended);
            }
            else if (recency.ShouldAccountBeLapsed())
            {
                SetAccountStatus(AccountStatusType.Lapsed);
            }
        }       

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (IAccountSnapshot)memento;

            accountStatus = snapshot.AccountStatus;
            recency = snapshot.Recency;
            clientId = snapshot.ClientId;
        }
    }
}