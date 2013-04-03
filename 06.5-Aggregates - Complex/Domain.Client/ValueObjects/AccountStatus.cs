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

    public class AccountStatus
    {
        public AccountStatusType Value { get; private set; }

        public AccountStatus(AccountStatusType value)
        {
            Value = value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AccountStatus);
        }

        public virtual bool Equals(AccountStatus other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Value == Value;
            }

            return false;
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