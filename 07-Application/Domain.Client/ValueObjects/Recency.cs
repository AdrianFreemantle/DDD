namespace Domain.Client.ValueObjects
{   
    public class Recency
    {
        public int Value { get; private set; }

        public Recency(int value)
        {
            Value = value;
        }

        public static Recency UpToDate()
        {
            return new Recency(0);
        }

        public Recency IncreaseRecency()
        {
            return new Recency(Value + 1);
        }

        public Recency FromBillingResult(BillingResult billingResult)
        {
            return billingResult.Paid ? Recency.UpToDate() : IncreaseRecency();
        }

        public bool IsUpToDate()
        {
            return Value == 0;
        }

        public bool ShouldAccountBeLapsed()
        {
            return Value == 6;
        }

        public bool ShouldAccountBeSuspended()
        {
            return Value == 3;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Recency);
        }

        public virtual bool Equals(Recency other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Value == Value;
            }

            return false;
        }

        public static bool operator ==(Recency left, Recency right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Recency left, Recency right)
        {
            return !Equals(left, right);
        } 
    }
}