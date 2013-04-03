using Domain.Client.Clients.Snapshots;
using Domain.Client.ValueObjects;
using Domain.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Tests.ClientTests
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class When_a_client_is_deceased : ClientTest
    {
        [TestMethod]
        public void Then_their_account_is_cancelled()
        {
            var client = DefaultClient(); 
            client.OpenAccount(DefaultAccountNumber);
            client.ClientIsDeceased();

            var snapshot = ((IEntity)client).GetSnapshot();
            AccountStatus accountStatus = ((ClientSnapshot)snapshot).AccountSnapshot.AccountStatus;

            accountStatus.Status.ShouldBe(AccountStatusType.Cancelled);
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
