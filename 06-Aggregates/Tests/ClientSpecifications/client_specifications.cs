using Domain.Client.Clients;
using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;

namespace Tests.ClientSpecifications
{
    public class client_specifications : AggregateTestSpecification<Client>
    {
        protected ClientRegistered ClientRegistered()
        {
            var idNumber = new IdentityNumber("5008035176089");
            var telephoneNumber = new TelephoneNumber("0125552222");
            var clientName = new PersonName("Adrian", "Freemantle");

            return new ClientRegistered(new ClientId(idNumber), idNumber, clientName, telephoneNumber);
        }
    }
}