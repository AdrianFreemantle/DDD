using System;
using Domain.Client.Clients.Snapshots;
using Domain.Client.ValueObjects;
using Domain.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Tests.ClientTests
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class When_a_client_is_billed : ClientTest
    {
        [TestMethod]
        public void Account_is_active_after_valid_payment()
        {
            var client = DefaultClient();
            client.OpenAccount(DefaultAccountNumber);
            client.RegisterPayment(BillingResult.PaymentMade(10, DateTime.Today));

            var snapshot = ((IEntity)client).GetSnapshot();
            AccountStatus accountStatus = ((ClientSnapshot)snapshot).AccountSnapshot.AccountStatus;

            accountStatus.Status.ShouldBe(AccountStatusType.Active);
        }

        [TestMethod]
        public void Account_is_suspended_after_three_payments_have_been_missed()
        {
            var client = DefaultClient();
            client.OpenAccount(DefaultAccountNumber);
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-01-01")));
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-02-01")));
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-03-01")));

            var snapshot = ((IEntity)client).GetSnapshot();
            AccountStatus accountStatus = ((ClientSnapshot)snapshot).AccountSnapshot.AccountStatus;

            accountStatus.Status.ShouldBe(AccountStatusType.Suspended);
        }

        [TestMethod]
        public void Account_is_lapsed_after_six_payments_have_been_missed()
        {
            var client = DefaultClient();
            client.OpenAccount(DefaultAccountNumber);
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-01-01")));
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-02-01")));
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-03-01")));
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-04-01")));
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-05-01")));
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-06-01")));

            var snapshot = ((IEntity)client).GetSnapshot();
            AccountStatus accountStatus = ((ClientSnapshot)snapshot).AccountSnapshot.AccountStatus;

            accountStatus.Status.ShouldBe(AccountStatusType.Lapsed);
        }
    }
}
