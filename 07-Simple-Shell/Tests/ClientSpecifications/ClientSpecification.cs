using Domain.Client.Clients;
using Domain.Client.Events;
using Domain.Client.ValueObjects;
using Domain.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.TestHelpers;

namespace Tests.ClientSpecifications
{
    public abstract class ClientSpecification : AggregateTestSpecification<Client>
    {
        public readonly IdentityNumber DefaultIdentityNumber = new IdentityNumber("7808035176089");
        public readonly PersonName DefaultClientName = new PersonName("Adrian", "Freemantle");
        public readonly TelephoneNumber DefaultTelephoneNumber = new TelephoneNumber("0125552222");
        public readonly ClientId DefaultClientId = new ClientId(new IdentityNumber("7808035176089"));

        [TestInitialize]
        public virtual void TestInit()
        {
            DomainEvent.Current.ClearSubscribers();
        }

        protected IDomainEvent ClientRegistered()
        {
            return new ClientRegistered(DefaultClientId, DefaultIdentityNumber, DefaultClientName, DefaultTelephoneNumber);
        }
    }
}