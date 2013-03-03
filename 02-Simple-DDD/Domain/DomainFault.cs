using System;

namespace Domain
{
    public class DomainFault : Exception
    {
        public DomainFault(string message)
            :base(message)
        {
        }
    }
}