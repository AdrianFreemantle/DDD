using System;
using System.Linq;
using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.TestHelpers;

namespace Tests.ClientTests
{
    public abstract class ClientTest
    {
        public readonly IdentityNumber DefaultIdentityNumber = new IdentityNumber("7808035176089");
        public readonly PersonName DefaultPersonName = new PersonName("Adrian", "Freemantle");
        public readonly TelephoneNumber DefaultTelephoneNumber = new TelephoneNumber("0125552222");
        public readonly ClientId DefaultClientId = new ClientId(new IdentityNumber("7808035176089"));
        public readonly AccountNumber DefaultAccountNumber = new AccountNumber("1234565");

        internal EventHandlerStub Events { get; set; }

        [TestInitialize]
        public virtual void TestInit()
        {
            Events = new EventHandlerStub();
            DomainEvent.Current.ClearSubscribers();
        }

        protected Client DefaultClient()
        {
            return Client.RegisterClient(DefaultIdentityNumber, DefaultPersonName, DefaultTelephoneNumber);
        }

        protected Account DefaultAccount()
        {
            return Account.Open(DefaultClientId, DefaultAccountNumber);
        }

        protected virtual void Then(params IDomainEvent[] domainEvents)
        {
            if (Events.RaisedEvents.Count() != domainEvents.Count())
            {
                Assert.Fail("{0} events were raised but we expected {1} events ",
                            Events.RaisedEvents.Count(), domainEvents.Count());
            }

            AssertEquality(Events.RaisedEvents.ToArray(), domainEvents);
        }

        private void AssertEquality(IDomainEvent[] expected, IDomainEvent[] actual)
        {
            for (int index = 0; index < expected.Length; index++)
            {
                string result = CompareObjects.FindDifferences(expected[index], actual[index]);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    Assert.Fail("Expected event did not match the actual event.\n{0}", result);
                }
            }
        }
    }
}