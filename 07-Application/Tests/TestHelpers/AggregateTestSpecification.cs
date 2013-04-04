using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Domain.Core;
using Domain.Core.Events;
using Domain.Core.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.TestHelpers
{
    public interface IGiven
    {
        IGiven And(IDomainEvent @event);
    }

    public abstract class AggregateTestSpecification<TAggregate> : IGiven where TAggregate : class, IAggregate 
    {
        protected TAggregate Aggregate { get; private set; }
        private Exception thrownException;
        private EventBusStub eventBusStub;

        protected virtual TAggregate ConstructAggregate()
        {
            return ActivatorHelper.CreateInstance<TAggregate>();
        }

        [TestInitialize]
        public virtual void TestSetup()
        {
            eventBusStub = new EventBusStub();
            DomainEvent.Current.ClearSubscribers();
            DomainEvent.Current.RegisterEventBus(eventBusStub);
            Aggregate = ConstructAggregate();
        }

        IGiven IGiven.And(IDomainEvent @event)
        {
            Console.WriteLine("And  : {0}", GetFormattedEventName(@event));
            Aggregate.ApplyEvent(@event);
            return this;
        }

        protected IGiven Given(IDomainEvent @event)
        {
            Console.WriteLine("Given: {0}", GetFormattedEventName(@event));
            Aggregate.ApplyEvent(@event);
            return this;
        }

        private string GetFormattedEventName(IDomainEvent @event)
        {
            return InsertSpaces(@event.GetType().ToString().Split('.').Last());
        }

        protected void When(Expression<Func<TAggregate>> expression)
        {
            try
            {
                Console.WriteLine("When : {0}", GetFormattedFunctionName(expression));
                Aggregate = expression.Compile()();
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
            return InsertSpaces(name);
        }

        private static string InsertSpaces(string name)
        {
            return Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
        }

        protected void When(Expression<Action<TAggregate>> expression)
        {
            try
            {
                Console.WriteLine("When : {0}", GetFormattedActionName(expression));
                Action<TAggregate> action = expression.Compile();
                action(Aggregate);
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
            return InsertSpaces(name);
        }

        protected virtual void Then<TException>() where TException : Exception
        {
            if (thrownException == null)
                Assert.Fail("An exception of type {0} was expected but not thrown", typeof(TException).FullName);

            if (typeof(TException) != thrownException.GetType())
                Assert.Fail("The expected exception type does not match the thrown type. \n\tExpected Exception : {0} \n\tActual Thrown      : {1}", typeof(TException).FullName, thrownException.GetType().FullName);

            Console.WriteLine("Then : {0} : {1}", typeof(TException).Name, thrownException.Message);
        }

        protected virtual void Then<TException>(string expectedName) where TException : DomainError
        {
            Then<TException>();

            if (!((DomainError)thrownException).Name.Equals(expectedName, StringComparison.Ordinal))
                Assert.Fail("The expected exception name does not match the expected name . \n\tExpected Name : {0} \n\tActual NAme   : {1}", expectedName, ((DomainError)thrownException).Name);

            Console.WriteLine("Then : {0} : {1} : {2}", typeof(TException).Name, expectedName, thrownException.Message);
        }

        protected virtual void Then(params IDomainEvent[] domainEvents)
        {
            if (thrownException != null)
                throw thrownException;

            if (eventBusStub.RaisedEvents.Count() != domainEvents.Count())
                Assert.Fail("The aggregate {0} contained {1} events : Expected number of events is {2}",
                    Aggregate.GetType().Name, eventBusStub.RaisedEvents.Count(), domainEvents.Count());

            AssertEquality(eventBusStub.RaisedEvents.ToArray(), domainEvents);

            bool insertComma = false;

            foreach (var domainEvent in domainEvents)
            {
                if (insertComma)
                {
                    Console.Write(", {0}", GetFormattedEventName(domainEvent));
                }
                else
                {
                    insertComma = true;
                    Console.Write("Then : {0}", GetFormattedEventName(domainEvent));
                }
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