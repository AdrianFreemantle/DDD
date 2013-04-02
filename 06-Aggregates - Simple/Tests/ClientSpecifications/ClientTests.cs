using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ClientSpecifications
{
    public abstract class ClientTests
    {
        public readonly IdentityNumber DefaultIdentityNumber = new IdentityNumber("7808035176089");
        public readonly PersonName DefaultPersonName = new PersonName("Adrian", "Freemantle");
        public readonly TelephoneNumber DefaultTelephoneNumber = new TelephoneNumber("0125552222");

        [TestInitialize]
        public virtual void TestInit()
        {
            DomainEvent.Current.ClearSubscribers();
        }

        protected Client DefaultClient()
        {
            return Client.RegisterClient(DefaultIdentityNumber, DefaultPersonName, DefaultTelephoneNumber);
        }
    }
}