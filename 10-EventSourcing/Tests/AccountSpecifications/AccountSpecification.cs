using Domain.Client.Accounts;
using Domain.Client.Accounts.Events;
using Domain.Client.Clients;
using Domain.Client.ValueObjects;
using Domain.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.TestHelpers;

namespace Tests.AccountSpecifications
{
    public abstract class AccountSpecification : AggregateTestSpecification<Account>
    {
        public readonly ClientId DefaultClientId = new ClientId(new IdentityNumber("7808035176089"));
        public readonly AccountNumber DefaultAccountNumber = new AccountNumber("123456");

        [TestInitialize]
        public virtual void TestInit()
        {
            EventPublisher.Current.ClearSubscribers();
        }

        protected IDomainEvent AccountOpened()
        {
            return new AccountOpened(DefaultAccountNumber, DefaultClientId);
        }
    }
}