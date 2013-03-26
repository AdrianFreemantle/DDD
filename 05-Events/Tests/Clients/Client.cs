using System.Collections.Generic;
using System.Linq;

using Domain.Core;
using Domain.Core.Events;

using Tests.ValueObjects;

namespace Tests.Clients
{
    public class Client : Entity<ClientId>
    {
        private PersonName clientName;
        private TelephoneNumber primaryContactNumber;
        private DateOfBirth dateOfBirth;
        private HashSet<InsuranceProduct> insuranceProducts;

        protected Client(IHaveIdentity identity)
        {
            Identity = (ClientId)identity;
        }

        public Client(IdentityNumber idNumber, PersonName clientName, TelephoneNumber telephoneNumber)
        {
            Identity = new ClientId(idNumber);
            this.clientName = clientName;
            primaryContactNumber = telephoneNumber;

            insuranceProducts = new HashSet<InsuranceProduct>();
            dateOfBirth = idNumber.GetDateOfBirth();

            DomainEvent.Current.Raise(new ClientRegistered(Identity.Id, clientName.FirstName, clientName.Surname, telephoneNumber.Number));
        }

        public void CorrectDateOfBirth(DateOfBirth dateOfBirth)
        {
            this.dateOfBirth = dateOfBirth;
            RaiseEvent(new ClientDateOfBirthCorrected(Identity.Id, dateOfBirth.Date));
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
                ClientName = clientName,
                InsuranceProducts = insuranceProducts.ToArray(),
                PrimaryContactNumber = primaryContactNumber
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (ClientSnapshot)memento;

            dateOfBirth = snapshot.DateOfBirth;
            clientName = snapshot.ClientName;
            insuranceProducts = new HashSet<InsuranceProduct>(snapshot.InsuranceProducts);
            primaryContactNumber = snapshot.PrimaryContactNumber;
        }
    }
}