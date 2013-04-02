// ReSharper disable InconsistentNaming
using System;

using Domain.Client.Clients;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ClientSpecifications
{
    [TestClass]
    public class When_client_date_of_birth_is_corrected : ClientTests
    {
        [TestMethod]
        public void With_a_valid_bith_date()
        {
            var dateOfBirth = new DateOfBirth(DateTime.Today.Date.AddYears(-18));

            Given(ClientRegistered());
            When(client => client.CorrectDateOfBirth(dateOfBirth));
            Then(new ClientDateOfBirthCorrected(Aggregate.Identity.GetId(), dateOfBirth));
        }

        [TestMethod]
        public void With_an_invalid_birth_date()
        {
            var dateOfBirth = new DateOfBirth(DateTime.Today.Date);

            Given(ClientRegistered());
            When(client => client.CorrectDateOfBirth(dateOfBirth));
            Then<DomainError>();
        }

        [TestMethod]
        public void Blah()
        {
            var accountNumber = new AccountNumber("12345");
            var billingDate = new BillingDate(SalaryPaymentType.Monthly);

            Given(ClientRegistered());
            When(client => client.OpenAccount(accountNumber, billingDate));
            Then(new AccountOpened(accountNumber, billingDate));
        }
    }
}
// ReSharper restore InconsistentNaming
