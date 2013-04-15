using System;
using Domain.Client.Accounts.Events;
using Domain.Client.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.AccountSpecifications
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class When_an_account_is_billed : AccountSpecification
    {
        [TestMethod]
        public void Account_is_active_after_valid_payment()
        {
            Given(AccountOpened());
            When(account => account.RegisterPayment(BillingResult.PaymentMade(10, DateTime.Today)));
            Then(
                new AccountBilled(DefaultAccountNumber, DefaultClientId, Recency.UpToDate()),
                new AccountStatusChanged(DefaultAccountNumber, DefaultClientId, new AccountStatus(AccountStatusType.Active))
                );
        }

        [TestMethod]
        public void Account_is_suspended_after_three_payments_have_been_missed()
        {
            Given(AccountOpened()).And(new AccountBilled(DefaultAccountNumber, DefaultClientId, new Recency(2)));

            When(account => account.RegisterPayment(BillingResult.NotPaid(DateTime.Today)));
            
            Then(
                new AccountBilled(DefaultAccountNumber, DefaultClientId, new Recency(3)),
                new AccountStatusChanged(DefaultAccountNumber, DefaultClientId, new AccountStatus(AccountStatusType.Suspended))
                );
        }

        [TestMethod]
        public void Account_is_lapsed_after_six_payments_have_been_missed()
        {
            Given(AccountOpened()).And(new AccountBilled(DefaultAccountNumber, DefaultClientId, new Recency(5)));

            When(account => account.RegisterPayment(BillingResult.NotPaid(DateTime.Today)));

            Then(
                new AccountBilled(DefaultAccountNumber, DefaultClientId, new Recency(6)),
                new AccountStatusChanged(DefaultAccountNumber, DefaultClientId, new AccountStatus(AccountStatusType.Lapsed))
                );
        }
    }
}
