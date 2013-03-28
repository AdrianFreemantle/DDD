using System.Collections.Generic;
using System.Linq;

using Domain.Core;
using Domain.Core.Events;

using Tests.ValueObjects;

namespace Tests.Clients
{
    public class Client : Aggregate<ClientId>
    {
        private PersonName clientName;
        private TelephoneNumber primaryContactNumber;
        private DateOfBirth dateOfBirth;
        private HashSet<InsuranceProduct> insuranceProducts;

        protected Client()
        {
            insuranceProducts = new HashSet<InsuranceProduct>();
        }

        public static Client RegisterClient(IdentityNumber idNumber, PersonName clientName, TelephoneNumber telephoneNumber)
        {
            var client = new Client();
            client.RaiseEvent(new ClientRegistered(idNumber.Number, clientName.FirstName, clientName.Surname, telephoneNumber.Number));
            return client;
        }

        public void When(ClientRegistered @event)
        {
            var identityNumber = new IdentityNumber(@event.ClientId);
            Identity = new ClientId(identityNumber);
            clientName = new PersonName(@event.FirstName, @event.Surname);
            primaryContactNumber = new TelephoneNumber(@event.TelephoneNumber);
            dateOfBirth = identityNumber.GetDateOfBirth();
        }

        public void CorrectDateOfBirth(DateOfBirth dateOfBirth)
        {
            RaiseEvent(new ClientDateOfBirthCorrected(Identity.Id, dateOfBirth.Date));
        }

        public void When(ClientDateOfBirthCorrected @event)
        {
            dateOfBirth = new DateOfBirth(@event.DateOfBirth);
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