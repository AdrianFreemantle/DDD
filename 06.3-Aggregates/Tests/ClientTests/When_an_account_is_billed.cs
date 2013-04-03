using System;
using Domain.Client.Accounts;
using Domain.Client.Events;
using Domain.Client.ValueObjects;
using Domain.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ClientTests
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class When_an_account_is_billed : ClientTest
    {
        [TestMethod]
        public void Account_is_active_after_valid_payment()
        {
            DomainEvent.Current.Subscribe<AccountBilled>(Events.Handle);
            DomainEvent.Current.Subscribe<AccountStatusChanged>(Events.Handle);

            Account account = DefaultAccount();
            account.RegisterPayment(BillingResult.PaymentMade(10, DateTime.Today));

            Then(
                new AccountBilled(account.Identity, Recency.UpToDate()),
                new AccountStatusChanged(account.Identity, new AccountStatus(AccountStatusType.Active))
                );
        }

        [TestMethod]
        public void Account_is_suspended_after_three_payments_have_been_missed()
        {
            DomainEvent.Current.Subscribe<AccountBilled>(Events.Handle);
            DomainEvent.Current.Subscribe<AccountStatusChanged>(Events.Handle);

            Account account = DefaultAccount();
            account.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-01-01")));
            account.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-02-01")));
            account.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-03-01")));

            Then(
                new AccountBilled(account.Identity, new Recency(1)),
                new AccountBilled(account.Identity, new Recency(2)),
                new AccountBilled(account.Identity, new Recency(3)),
                new AccountStatusChanged(account.Identity, new AccountStatus(AccountStatusType.Suspended))
                );
        }

        [TestMethod]
        public void Account_is_lapsed_after_six_payments_have_been_missed()
        {
            DomainEvent.Current.Subscribe<AccountBilled>(Events.Handle);
            DomainEvent.Current.Subscribe<AccountStatusChanged>(Events.Handle);

            Account account = DefaultAccount();
            account.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-01-01")));
            account.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-02-01")));
            account.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-03-01")));
            account.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-04-01")));
            account.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-05-01")));
            account.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-06-01")));

            Then(
                new AccountBilled(account.Identity, new Recency(1)),
                new AccountBilled(account.Identity, new Recency(2)),
                new AccountBilled(account.Identity, new Recency(3)),
                new AccountStatusChanged(account.Identity, new AccountStatus(AccountStatusType.Suspended)),
                new AccountBilled(account.Identity, new Recency(4)),
                new AccountBilled(account.Identity, new Recency(5)),
                new AccountBilled(account.Identity, new Recency(6)),
                new AccountStatusChanged(account.Identity, new AccountStatus(AccountStatusType.Lapsed))
                );
        }
    }
}
