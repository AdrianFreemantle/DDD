using System;

namespace Domain.Core
{
    /// <summary>
    /// Base implementation of <see cref="IHaveIdentity"/>, which implements
    /// equality and ToString once and for all.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public abstract class IdentityBase<TKey> : IHaveIdentity
    {
        public abstract TKey Id { get; protected set; }
        public abstract string GetTag();

        public string GetId()
        {
            return Id.ToString();
        }
        
        public bool IsEmpty()
        {
            return default(TKey).Equals(Id);
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
            return (Id.GetHashCode());
        }

        public int GetStableHashCode()
        {
            // same as hash code, but works across multiple architectures 
            var type = typeof(TKey);

            return type == typeof(string) 
                ? CalculateStringHash(Id.ToString()) 
                : Id.GetHashCode();
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

        static int CalculateStringHash(string value)
        {
            if (value == null) return 42;
            
            unchecked
            {
                var hash = 23;

                foreach (var c in value)
                {
                    hash = hash * 31 + c;
                }

                return hash;
            }
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