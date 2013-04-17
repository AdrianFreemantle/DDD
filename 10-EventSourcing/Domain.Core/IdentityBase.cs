using System;
using Domain.Core.Infrastructure;

namespace Domain.Core
{
    /// <summary>
    /// Base implementation of <see cref="IHaveIdentity"/>, which implements
    /// equality and ToString once and for all.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public abstract class IdentityBase<TKey> : IHaveIdentity
    {
        // ReSharper disable StaticFieldInGenericType
        private static readonly Guid IdentityNamespace = Guid.Parse("24142086-67e1-5c77-aadf-6bd8d09c06b9");
        // ReSharper restore StaticFieldInGenericType
        private Guid surrogateId;
        public virtual TKey Id { get; protected set; }
        public abstract string GetTag();
        public abstract bool IsEmpty();

        public string GetId()
        {
            return Id.ToString();
        }

        //The surrogate identity is used in event sourcing to identify the event stream.
        Guid IHaveIdentity.GetSurrogateId()
        {
            return GetSurrogateId();
        }

        protected virtual Guid GetSurrogateId()
        {
            if (Id is Guid)
            {
                return (Guid)(object)Id;
            }

            if (surrogateId != Guid.Empty)
            {
                return surrogateId;
            }

            surrogateId = StringToGuid.Convert(ToString(), IdentityNamespace);
            return surrogateId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var identity = obj as IdentityBase<TKey>;

            return identity != null && Equals(identity);
        }

        public bool Equals(IHaveIdentity other)
        {
            return Equals((object)other);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", GetType().Name.Replace("Id", ""), Id);
        }

        public override int GetHashCode()
        {
            return (GetSurrogateId().GetHashCode());
        }

        static IdentityBase()
        {
            var type = typeof(TKey);

            if (type == typeof(int) ||
                type == typeof(long) ||
                type == typeof(uint) ||
                type == typeof(ulong) ||
                type == typeof(Guid) ||
                type == typeof(string))
            {
                return;
            }

            throw new InvalidOperationException("Abstract identity inheritors must provide stable hash. It is not supported for:  " + type);
        }

        public bool Equals(IdentityBase<TKey> other)
        {
            if (other != null)
            {
                return other.Id.Equals(Id) && other.GetTag() == GetTag();
            }

            return false;
        }

        public static bool operator ==(IdentityBase<TKey> left, IdentityBase<TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IdentityBase<TKey> left, IdentityBase<TKey> right)
        {
            return !Equals(left, right);
        }
    }
}