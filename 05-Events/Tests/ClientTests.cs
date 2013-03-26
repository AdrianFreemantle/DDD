using System;
using System.Linq;

using Domain.Core;
using Domain.Core.Events;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shouldly;

using Tests.Clients;
using Tests.ValueObjects;

namespace Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            DomainEvent.Current.ClearCallbacks();
        }

        private static Client CreateGenericClient()
        {
            var idNumber = new IdentityNumber("5008035176089");
            var telephoneNumber = new TelephoneNumber("0125552222");
            var clientName = new PersonName("Adrian", "Freemantle");

            return new Client(idNumber, clientName, telephoneNumber);
        }

        [TestMethod]
        public void A_clients_with_same_identity_number_are_the_same_client()
        {
            var idNumber = new IdentityNumber("7808035176089");
            var telephoneNumber = new TelephoneNumber("0125552222");
            var clientName = new PersonName("Adrian", "Freemantle");
            var client = new Client(idNumber, clientName, telephoneNumber);

            var telephoneNumber2 = new TelephoneNumber("1111111111");
            var clientName2 = new PersonName("Sally", "Smith");
            var client2 = new Client(idNumber, clientName2, telephoneNumber2);

            client.ShouldBe(client2);
        }

        [TestMethod]
        public void Clients_over_60_with_three_or_more_insurance_products_qualify_for_pensioners_discount()
        {
            var client = CreateGenericClient();

            client.Purchased(new InsuranceProduct());
            client.Purchased(new InsuranceProduct());
            client.Purchased(new InsuranceProduct());
            
            client.QualifiesForPensionersDiscount().ShouldBe(true);
        }

        [TestMethod]
        public void Can_restore_from_snapshot()
        {
            var client = CreateGenericClient();

            var originalSnapshot = (client as IEntity).GetSnapshot();
            var restored = EntityFactory.Build<Client>(originalSnapshot);
            var restoredSnapshot = (restored as IEntity).GetSnapshot();

            var comparer = new CompareObjects();
            comparer.Compare(originalSnapshot, restoredSnapshot).ShouldBe(true);
        }

        [TestMethod]
        public void A_new_client_can_be_registered()
        {
            var eventHelper = new EventHandlerHelper();
            DomainEvent.Current.Register<ClientRegistered>(eventHelper.Handle);

            var idNumber = new IdentityNumber("5008035176089");
            var telephoneNumber = new TelephoneNumber("0125552222");
            var clientName = new PersonName("Adrian", "Freemantle");
            new Client(idNumber, clientName, telephoneNumber);

            eventHelper.RaisedEvents.Count.ShouldBe(1);
            eventHelper.RaisedEvents.First().ShouldBeTypeOf<ClientRegistered>();
            ((ClientRegistered)eventHelper.RaisedEvents.First()).ClientId.ShouldBe(idNumber.Number);
            ((ClientRegistered)eventHelper.RaisedEvents.First()).FirstName.ShouldBe(clientName.FirstName);
            ((ClientRegistered)eventHelper.RaisedEvents.First()).Surname.ShouldBe(clientName.Surname);
            ((ClientRegistered)eventHelper.RaisedEvents.First()).TelephoneNumber.ShouldBe(telephoneNumber.Number);
        }        

        [TestMethod]
        public void Correcting_a_date_of_birth_raises_a_ClientDateOfBirthCorrected_event()
        {
            var client = CreateGenericClient();
            var dateOfBirth = new DateOfBirth(new DateTime(1920, 01, 01));
            client.CorrectDateOfBirth(dateOfBirth);

            client.GetRaisedEvents().Count().ShouldBe(1);
            client.GetRaisedEvents().First().ShouldBeTypeOf<ClientDateOfBirthCorrected>();
        }
    }
}
