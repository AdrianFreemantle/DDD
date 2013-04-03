using Domain.Client.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ClientSpecifications
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class When_a_client_is_deceased : ClientSpecification
    {
        [TestMethod]
        public void A_ClientPassedAway_event_is_raised()
        {
            Given(ClientRegistered());
            When(client => client.ClientIsDeceased());
            Then(new ClientPassedAway(DefaultClientId));
        }
    }
}
