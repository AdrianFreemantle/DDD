using System;

namespace Domain.Core
{
    /// <summary>
    /// Strongly-typed identity class. Essentially just an ID with a 
    /// distinct type. It introduces strong-typing if entity identity
    /// </summary>
    public interface IHaveIdentity : IEquatable<IHaveIdentity>
    {
        /// <summary>
        /// Gets the id, converted to a string. Only alphanumerics and '-' are allowed.
        /// </summary>
        /// <returns></returns>
        string GetId();

        /// <summary>
        /// Unique tag (should be unique within the assembly) to distinguish
        /// between different identities, while deserializing.
        /// </summary>
        string GetTag();

        bool IsEmpty();

        Guid GetSurrogateId();
    }
}
