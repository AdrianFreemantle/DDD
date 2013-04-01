using System;

using Domain.Client.Clients;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ClientSpecifications
{
    [TestClass]
    public class When_client_date_of_birth_is_corrected : client_specifications
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
    }
}