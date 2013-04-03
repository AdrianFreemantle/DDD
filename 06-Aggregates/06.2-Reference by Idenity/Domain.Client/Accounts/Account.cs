using System.Collections.Generic;
using System.Linq;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Accounts
{
    public class Account : Aggregate<AccountNumber>
    {
        private AccountStatus accountStatus;
        private Recency recency;

        private ClientId clientId;

        protected Account()
        {
            recency = Recency.UpToDate();
            SetAccountStatus(AccountStatusType.Active);
        }

        internal static Account Open(ClientId clientId, AccountNumber accountNumber)
        {
            Mandate.ParameterNotNull(accountNumber, "accountNumber");
            Mandate.ParameterNotNull(clientId, "clientId");

            return new Account
            {
                Identity = accountNumber,
                clientId = clientId
            };
        }

        public void Cancel()
        {
            SetAccountStatus(AccountStatusType.Cancelled);
        }

        private void SetAccountStatus(AccountStatusType status)
        {
            accountStatus = new AccountStatus(status);
        }

        public void RegisterPayment(BillingResult billingResult)
        {
            Mandate.ParameterNotNull(billingResult, "billingResult");

            recency = billingResult.Paid ? Recency.UpToDate() : recency.IncreaseRecency();

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

        protected override IMemento GetSnapshot()
        {
            return new AccountSnapshot
            {
                AccountStatus = accountStatus,
                Recency = recency
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (AccountSnapshot)memento;

            accountStatus = snapshot.AccountStatus;
            recency = snapshot.Recency;
        }
    }
}