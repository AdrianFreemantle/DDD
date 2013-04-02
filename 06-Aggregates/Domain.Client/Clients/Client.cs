using Domain.Client.Clients.Events;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{   
    public partial class Client : Aggregate<ClientId>
    {
        protected Client()
        {
        }

        public static Client RegisterClient(IdentityNumber idNumber, PersonName clientName, TelephoneNumber telephoneNumber)
        {
            var client = new Client();
            client.RaiseEvent(new ClientRegistered(new ClientId(idNumber), idNumber, clientName, telephoneNumber));
            return client;
        }

        public void OpenAccount(AccountNumber accountNumber, BillingDate billingDate)
        {
            if (account != null)
            {
                throw DomainError.Named("Account-Exists", "The account alreay exists");
            }
            
            account = Account.Open(accountNumber, billingDate);
            RaiseEvent((account as IEntity).GetRaisedEvents());
        }

        public void CorrectDateOfBirth(DateOfBirth dateOfBirth)
        {
            if (dateOfBirth.GetCurrentAge() < 18)
            {
                throw DomainError.Named("Under-Age", "A client must be older than 18 years.");
            }

            RaiseEvent(new ClientDateOfBirthCorrected(Identity.Id, dateOfBirth));
        }
    }
}