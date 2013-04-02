using System.Collections.Generic;
using System.Linq;
using Domain.Client.Clients.Snapshots;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{
    public class Account : Entity<AccountNumber>
    {
        private AccountStatus accountStatus;
        private HashSet<BillingResult> billingResults;

        protected Account()
        {
            billingResults = new HashSet<BillingResult>(); 
            SetAccountStatus(AccountStatusType.Active);
        }

        internal static Account Open(AccountNumber accountNumber)
        {
            Mandate.ParameterNotNull(accountNumber, "accountNumber");

            return new Account
            {
                Identity = accountNumber
            };
        }

        private void SetAccountStatus(AccountStatusType status)
        {
            accountStatus = new AccountStatus(status);
        }

        internal void RegisterPayment(BillingResult billingResult)
        {
            billingResults.Add(billingResult);
            UpdateStatusBasedOnRecency();
        }

        internal void ReversePayment(BillingResult billingResult)
        {
            billingResults.Remove(billingResult);
            UpdateStatusBasedOnRecency();
        }

        private void UpdateStatusBasedOnRecency()
        {
            int notPaidCounter = 0;

            foreach (var billingResult in billingResults)
            {
                if (billingResult.Paid)
                {
                    notPaidCounter = 0;
                }
                else
                {
                    notPaidCounter++;
                }
            }

            if (notPaidCounter < 3)
            {
                SetAccountStatus(AccountStatusType.Active);
            }
            else if (notPaidCounter == 3)
            {
                SetAccountStatus(AccountStatusType.Suspended);
            }
            else if (notPaidCounter == 6)
            {
                SetAccountStatus(AccountStatusType.Lapsed);
            }
        }

        protected override IMemento GetSnapshot()
        {
            return new AccountSnapshot
            {
                AccountStatus = accountStatus,
                BillingResults = billingResults.ToArray()
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (AccountSnapshot)memento;

            accountStatus = snapshot.AccountStatus;
            billingResults = new HashSet<BillingResult>(snapshot.BillingResults);
        }
    }
}