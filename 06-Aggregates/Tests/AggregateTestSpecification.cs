using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Core;
using Domain.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Clients;
using Tests.ValueObjects;

namespace Tests
{
    [TestClass]
    public class when_client_is_registered : client_specifications
    {
        [TestMethod]
        public void DoSomeThingsHere()
        {
            var idNumber = new IdentityNumber("5008035176089");
            var telephoneNumber = new TelephoneNumber("0125552222");
            var clientName = new PersonName("Adrian", "Freemantle");

            Given();
            When(Client.RegisterClient(idNumber, clientName, telephoneNumber));
            Then(new ClientRegistered(idNumber.Number, "susan", clientName.Surname, telephoneNumber.Number));
        }
    }

    public class client_specifications : AggregateTestSpecification<Client>
    {
        
    }

    public abstract class AggregateTestSpecification<TAggregate> where TAggregate : class, IAggregate 
    {
        protected TAggregate Aggregate { get; private set; }
        Exception thrownException;

        protected virtual TAggregate ConstructAggregate()
        {
            return ActivatorHelper.CreateInstance<TAggregate>();
        }

        [TestInitialize]
        public virtual void TestSetup()
        {
            Aggregate = ConstructAggregate();
        }

        protected void Given()
        {
        }       

        protected void Given(IDomainEvent @event)
        {
            ((dynamic)Aggregate).When((dynamic)@event);
        }

        protected void Given(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                ((dynamic)Aggregate).When((dynamic)@event);
            }
        }

        protected void When(TAggregate aggregate)
        {
            Aggregate = aggregate;
        }

        protected void When(Action<TAggregate> action)
        {
            try
            {
                action.Invoke(Aggregate);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }
        }

        protected virtual void Then<TException>() where TException : Exception
        {
            if (thrownException == null)
                Assert.Fail("An exception of type {0} was expected but not thrown", typeof(TException).FullName);

            if (typeof(TException) != thrownException.GetType())
                Assert.Fail("The expected exception type does not match the thrown type. \n\tExpected Exception : {0} \n\tActual Thrown      : {1}", typeof(TException).FullName, thrownException.GetType().FullName);
        }

        protected virtual void Then<TException>(string expectedMessage) where TException : Exception
        {
            Then<TException>();

            if (!thrownException.Message.Equals(expectedMessage, StringComparison.Ordinal))
                Assert.Fail("The expected exception message does not match the thrown message. \n\tExpected Message : {0} \n\tActual Message   : {1}", expectedMessage, thrownException.Message);
        }

        protected virtual void Then(params IDomainEvent[] domainEvents)
        {
            if (thrownException != null)
                throw thrownException;

            if (Aggregate.GetRaisedEvents().Count() != domainEvents.Count())
                Assert.Fail("The aggregate {0} contained {1} events : Expected number of events is {2}", 
                    Aggregate.GetType().Name, Aggregate.GetRaisedEvents().Count(), domainEvents.Count());

            AssertEquality(Aggregate.GetRaisedEvents().ToArray(), domainEvents);
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