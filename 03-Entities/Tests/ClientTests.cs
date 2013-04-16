using Domain.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shouldly;

using Tests.ValueObjects;

namespace Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void A_client_must_have_an_identity_number_contact_number_and_name()
        {
            var idNumber = new IdentityNumber("7808035176089");
            var telephoneNumber = new TelephoneNumber("0125552222");
            var clientName = new PersonName("Adrian", "Freemantle");

            var client = new Client(idNumber, clientName, telephoneNumber);

            client.Identity.Id.ShouldBe(idNumber.Number);
            client.PrimaryContactNumber.ShouldBe(telephoneNumber);
            client.ClientName.ShouldBe(clientName);
        }

        [TestMethod]
        public void A_clients_with_same_identity_nuber_are_the_same_client()
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
            var idNumber = new IdentityNumber("5008035176089");
            var telephoneNumber = new TelephoneNumber("0125552222");
            var clientName = new PersonName("Adrian", "Freemantle");
            var client = new Client(idNumber, clientName, telephoneNumber);

            client.Purchased(new InsuranceProduct());
            client.Purchased(new InsuranceProduct());
            client.Purchased(new InsuranceProduct());
            
            client.QualifiesForPensionersDiscount().ShouldBe(true);
        }

        [TestMethod]
        public void Can_restore_from_snapshot()
        {
            var idNumber = new IdentityNumber("5008035176089");
            var telephoneNumber = new TelephoneNumber("0125552222");
            var clientName = new PersonName("Adrian", "Freemantle");
            var client = new Client(idNumber, clientName, telephoneNumber);

            client.Purchased(new InsuranceProduct());
            client.Purchased(new InsuranceProduct());
            client.Purchased(new InsuranceProduct());

            var snapshot = (client as IEntity).GetSnapshot();

            var restored = EntityFactory.Build<Client>(snapshot);
        }
    }
}
