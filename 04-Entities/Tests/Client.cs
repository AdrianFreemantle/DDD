using System.Collections.Generic;

using Domain.Core;

using Tests.ValueObjects;

namespace Tests
{
    public class Client : Entity<ClientId>
    {
        public PersonName ClientName { get; set; }
        public TelephoneNumber PrimaryContactNumber { get; protected set; }

        private DateOfBirth dateOfBirth;
        private HashSet<InsuranceProduct> insuranceProducts;

        protected Client(IHaveIdentity identity)
        {
            Identity = (ClientId)identity;
        }

        public Client(IdentityNumber idNumber, PersonName clientName, TelephoneNumber telephoneNumber)
        {
            Identity = new ClientId(idNumber);
            ClientName = clientName;
            PrimaryContactNumber = telephoneNumber;

            insuranceProducts = new HashSet<InsuranceProduct>();
            dateOfBirth = idNumber.GetDateOfBirth();
        }

        public bool QualifiesForPensionersDiscount()
        {
            return dateOfBirth.GetCurrentAge() > 60 && insuranceProducts.Count >= 3;
        }

        public void Purchased(InsuranceProduct product)
        {
            insuranceProducts.Add(product);
        }

        protected override IMemento GetSnapshot()
        {
            return new ClientSnapshot
            {
                DateOfBirth = dateOfBirth,
                ClientName = ClientName,
                InsuranceProducts = insuranceProducts,
                PrimaryContactNumber = PrimaryContactNumber
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (ClientSnapshot)memento;

            dateOfBirth = snapshot.DateOfBirth;
            ClientName = snapshot.ClientName;
            insuranceProducts = snapshot.InsuranceProducts;
            PrimaryContactNumber = snapshot.PrimaryContactNumber;
        }
    }
}