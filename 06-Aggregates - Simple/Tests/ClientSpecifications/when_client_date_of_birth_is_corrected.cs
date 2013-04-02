// ReSharper disable InconsistentNaming
using System;

using Domain.Client.Clients;
using Domain.Client.Clients.Snapshots;
using Domain.Client.ValueObjects;
using Domain.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shouldly;

namespace Tests.ClientSpecifications
{
    [TestClass]
    public class When_client_date_of_birth_is_corrected : ClientTests
    {
        [TestMethod]
        public void With_a_valid_bith_date()
        {
            var dateOfBirth = new DateOfBirth(DateTime.Today.Date.AddYears(-18));

            var client = DefaultClient();
            client.CorrectDateOfBirth(dateOfBirth);

            var snapshot = ((IEntity)client).GetSnapshot();
            ((ClientSnapshot)snapshot).DateOfBirth.ShouldBe(dateOfBirth);
        }

        [TestMethod, ExpectedException(typeof(DomainError))]
        public void With_an_invalid_birth_date()
        {
            var dateOfBirth = new DateOfBirth(DateTime.Today.Date);

            var client = DefaultClient();
            client.CorrectDateOfBirth(dateOfBirth);
        }
    }
}
// ReSharper restore InconsistentNaming
