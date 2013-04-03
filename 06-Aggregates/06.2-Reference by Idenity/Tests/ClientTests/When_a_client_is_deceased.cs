using Domain.Client.Events;
using Domain.Core;
using Domain.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Tests.TestHelpers;

namespace Tests.ClientTests
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class When_a_client_is_deceased : ClientTest
    {
        [TestMethod]
        public void A_ClientPassedAway_event_is_raised()
        {
            var handlerStub = new EventHandlerStub();
            DomainEvent.Current.Subscribe<ClientPassedAway>(handlerStub.Handle);

            var client = DefaultClient();
            client.OpenAccount(DefaultAccountNumber);
            client.ClientIsDeceased();

            handlerStub.RaisedEvents.Count.ShouldBe(1);
            ((ClientPassedAway)handlerStub.RaisedEvents[0]).ClientId.ShouldBe(DefaultClientId);
        }

        [TestMethod, ExpectedException(typeof(DomainError))]
        public void An_account_cannot_be_opened()
        {
            try
            {
                var client = DefaultClient();
                client.ClientIsDeceased();
                client.OpenAccount(DefaultAccountNumber);
            }
            catch (DomainError e)
            {
                e.Name.ShouldBe("client-deceased");
                throw;
            }
        }
    }
}
