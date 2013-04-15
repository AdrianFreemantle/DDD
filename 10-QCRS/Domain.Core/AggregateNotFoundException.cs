using System;
using System.Runtime.Serialization;

namespace Domain.Core
{
    [Serializable]
    public class AggregateNotFoundException : Exception
    {
        public string Name { get; private set; }

        public AggregateNotFoundException(string message)
            : base(message)
        {
        }

        protected AggregateNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}