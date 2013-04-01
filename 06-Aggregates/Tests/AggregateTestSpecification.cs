using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using Domain.Core;
using Domain.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
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
            Console.WriteLine("Given: {0}", @event);
            Aggregate.ApplyEvent(@event);
        }

        protected void Given(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                Given(@event);
            }
        }

        protected void When(Expression<Func<TAggregate>> function)
        {
            try
            {
                Console.WriteLine("When : {0}", GetFormattedFunctionName(function));
                Aggregate = function.Compile()();
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }
        }

        private string GetFormattedFunctionName(Expression<Func<TAggregate>> function)
        {
            string name = function.ToString().Replace("() => ", "");
            int firstBracket = name.IndexOf('(');

            name = name.Remove(firstBracket);
            return Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
        }

        protected void When(Expression<Action<TAggregate>> action)
        {
            try
            {
                Console.WriteLine("When : {0}", GetFormattedActionName(action));
                action.Compile()(Aggregate);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }
        }

        private string GetFormattedActionName(Expression<Action<TAggregate>> action)
        {
            string name = action.ToString();
            int firstDot = name.IndexOf('.');
            int firstBracket = name.IndexOf('(');

            name = name.Remove(firstBracket).Substring(firstDot + 1);
            return Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
        }

        protected virtual void Then<TException>() where TException : Exception
        {
            if (thrownException == null)
                Assert.Fail("An exception of type {0} was expected but not thrown", typeof(TException).FullName);

            if (typeof(TException) != thrownException.GetType())
                Assert.Fail("The expected exception type does not match the thrown type. \n\tExpected Exception : {0} \n\tActual Thrown      : {1}", typeof(TException).FullName, thrownException.GetType().FullName);

            Console.WriteLine("Then : {0} : {1}", typeof(TException).Name, thrownException.Message);
        }

        protected virtual void Then<TException>(string expectedMessage) where TException : Exception
        {
            Then<TException>();

            if (!thrownException.Message.Equals(expectedMessage, StringComparison.Ordinal))
                Assert.Fail("The expected exception message does not match the thrown message. \n\tExpected Message : {0} \n\tActual Message   : {1}", expectedMessage, thrownException.Message);

            Console.WriteLine("Then : {0} : {1}", typeof(TException).Name, expectedMessage);
        }

        protected virtual void Then(params IDomainEvent[] domainEvents)
        {
            if (thrownException != null)
                throw thrownException;

            if (Aggregate.GetRaisedEvents().Count() != domainEvents.Count())
                Assert.Fail("The aggregate {0} contained {1} events : Expected number of events is {2}", 
                    Aggregate.GetType().Name, Aggregate.GetRaisedEvents().Count(), domainEvents.Count());

            AssertEquality(Aggregate.GetRaisedEvents().ToArray(), domainEvents);

            foreach (var domainEvent in domainEvents)
            {
                Console.WriteLine("Then : {0}", domainEvent);
            }
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