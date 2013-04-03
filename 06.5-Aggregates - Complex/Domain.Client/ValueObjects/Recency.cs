namespace Domain.Client.ValueObjects
{   
    public class Recency
    {
        public int Value { get; private set; }

        public Recency(int value)
        {
            Value = value;
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