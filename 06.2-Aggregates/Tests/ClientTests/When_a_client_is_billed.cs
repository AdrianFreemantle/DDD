// ReSharper disable InconsistentNaming

using System;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Tests.ClientTests
{
    [TestClass]
    public class When_a_client_is_billed : ClientTest
    {
        [TestMethod]
        public void And_payment_has_been_made()
        {
            var client = DefaultClient();
            client.OpenAccount(DefaultAccountNumber);
            client.RegisterPayment(BillingResult.PaymentMade(10, DateTime.Today));

            var snapshot = ((IEntity)client).GetSnapshot();
            AccountStatus accountStatus = ((ClientSnapshot)snapshot).AccountSnapshot.AccountStatus;

            accountStatus.Status.ShouldBe(AccountStatusType.Active);
        }

        [TestMethod]
        public void And_three_payments_have_been_missed()
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
        public void And_six_payments_have_been_missed()
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

        [TestMethod]
        public void The_payment_can_be_reversed()
        {
            var client = DefaultClient();
            client.OpenAccount(DefaultAccountNumber);
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-01-01")));
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-02-01")));
            client.RegisterPayment(BillingResult.NotPaid(DateTime.Parse("2005-03-01")));

            var payment = BillingResult.PaymentMade(10, DateTime.Parse("2005-04-01"));

            client.RegisterPayment(payment);
            client.ReversePayment(payment);

            var snapshot = ((IEntity)client).GetSnapshot();
            AccountStatus accountStatus = ((ClientSnapshot)snapshot).AccountSnapshot.AccountStatus;

            accountStatus.Status.ShouldBe(AccountStatusType.Suspended);
        }
    }
}
// ReSharper restore InconsistentNaming