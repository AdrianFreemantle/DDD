using System;
using System.Runtime.Serialization;
namespace Domain.Client.ValueObjects
{
    public enum AccountStatusType
    {
        Unknown,
        Active,
        Reinstated,
        Suspended,
        Lapsed,
        Cancelled
    }

    [DataContract]
    public struct AccountStatus : IEquatable<AccountStatus>
    {
        [DataMember(Order = 1, Name = "Status", IsRequired = true)]
        public AccountStatusType Status { get; private set; }

        public AccountStatus(AccountStatusType value)
            : this()
        {
            Status = value;
        }

        public bool StatusMayBeChanged()
        {
            return Status != AccountStatusType.Lapsed;
        }

        public override int GetHashCode()
        {
            return Status.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is AccountStatus && Equals((AccountStatus)obj);
        }

        public bool Equals(AccountStatus other)
        {
            return other.Status == Status;
        }

        public static bool operator ==(AccountStatus left, AccountStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AccountStatus left, AccountStatus right)
        {
            return !Equals(left, right);
        }
    }
}