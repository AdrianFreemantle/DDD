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

    public struct AccountStatus
    {
        public AccountStatusType Status { get; private set; }

        public AccountStatus(AccountStatusType value) : this()
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
            return Equals((AccountStatus)obj);
        }

        public bool Equals(AccountStatus other)
        {
            if (other.GetType() == GetType())
            {
                return other.Status == Status;
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