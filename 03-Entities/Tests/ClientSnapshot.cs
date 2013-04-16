using System.Collections.Generic;

using Domain.Core;

using Tests.ValueObjects;

namespace Tests
{
    public class ClientSnapshot : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public PersonName ClientName { get; set; }
        public TelephoneNumber PrimaryContactNumber { get; set; }
        public DateOfBirth DateOfBirth { get; set; }
        public HashSet<InsuranceProduct> InsuranceProducts { get; set; }
    }
}