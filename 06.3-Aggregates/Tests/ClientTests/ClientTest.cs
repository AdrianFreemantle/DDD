using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ClientTests
{
    public abstract class ClientTest
    {
        public readonly IdentityNumber DefaultIdentityNumber = new IdentityNumber("7808035176089");
        public readonly PersonName DefaultPersonName = new PersonName("Adrian", "Freemantle");
        public readonly TelephoneNumber DefaultTelephoneNumber = new TelephoneNumber("0125552222");
        public readonly AccountNumber DefaultAccountNumber = new AccountNumber("1234565");

        [TestCleanup]
        public virtual void TestCleanup()
        {
            DomainEvent.Current.ClearSubscribers();
        }

        protected Client DefaultClient()
        {
            return Client.RegisterClient(DefaultIdentityNumber, DefaultPersonName, DefaultTelephoneNumber);
        }
    }
}