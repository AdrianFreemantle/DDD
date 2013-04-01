using System;

namespace Domain.Client.ValueObjects
{
    public class TelephoneNumber : IEquatable<TelephoneNumber>
    {
        public string Number { get; private set; }

        public TelephoneNumber(string number)
        {
            Number = number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TelephoneNumber);
        }

        public virtual bool Equals(TelephoneNumber other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Number.Equals(Number);
            }

            return false;
        }

        public static bool operator ==(TelephoneNumber left, TelephoneNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TelephoneNumber left, TelephoneNumber right)
        {
            return !Equals(left, right);
        }
    }
}