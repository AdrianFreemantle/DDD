using Domain.Client.Clients.Snapshots;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{
    public class Account : Entity<AccountNumber>
    {
        private Recency recency;
        private BillingDate billingDate;

        protected Account()
        {
        }

        protected Account(AccountNumber accountNumber, BillingDate billingDate)
        {
            Identity = accountNumber;
            this.billingDate = billingDate;
        }

        internal static Account Open(AccountNumber accountNumber, BillingDate billingDate)
        {
            return new Account
            {
                Identity = accountNumber,
                billingDate = billingDate
            };
        }

        internal static Account Null()
        {
            return new Account
            {
                Identity = new AccountNumber("00000000"),
                billingDate = new BillingDate(SalaryPaymentType.Unknown)
            };
        }

        protected override IMemento GetSnapshot()
        {
            return new AccountSnapshot
            {
                Recency = recency,
                BillingDate = billingDate
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (AccountSnapshot)memento;

            recency = snapshot.Recency;
            billingDate = snapshot.BillingDate;
        }
    }
}