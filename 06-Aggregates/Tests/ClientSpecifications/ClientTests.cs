using Domain.Client.Clients;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Tests.TestHelpers;

namespace Tests.ClientSpecifications
{
    public class ClientTests : AggregateTestSpecification<Client>
    {
        public readonly IdentityNumber DefaultIdentityNumber = new IdentityNumber("7808035176089");
        public readonly PersonName DefaultPersonName = new PersonName("Adrian", "Freemantle");
        public readonly TelephoneNumber DefaultTelephoneNumber = new TelephoneNumber("0125552222");

        protected ClientRegistered ClientRegistered()
        {
            return new ClientRegistered(new ClientId(DefaultIdentityNumber), DefaultIdentityNumber, DefaultPersonName, DefaultTelephoneNumber);
        }
    }
}