using System;
using System.Runtime.Serialization;

namespace Domain.Core
{
    /// <summary>
    /// Special exception that is thrown by application services
    /// when something goes wrong in an expected way. This exception
    /// bears human-readable code (the name property acting as sort of a 'key') which is used to verify it
    /// in the tests.
    /// An Exception name is a hard-coded identifier that is still human readable but is not likely to change.
    /// </summary>
    [Serializable]
    public class DomainError : Exception
    {
        public string Name { get; private set; }

        public DomainError(string message) 
            : base(message)
        {
        }

        protected DomainError(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Creates domain error exception with a string name that is easily identifiable in the tests
        /// </summary>
        /// <param name="name">The name to be used to identify this exception in tests.</param>
        /// <param name="message">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static DomainError Named(string name, string message, params object[] args)
        {
            return new DomainError(String.Format(message, args))
            {
                Name = name
            };
        }
    }
}