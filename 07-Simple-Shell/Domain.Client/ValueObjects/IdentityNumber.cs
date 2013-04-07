using System;

namespace Domain.Client.ValueObjects
{
    public class IdentityNumber
    {
        public string Number { get; private set; }

        public IdentityNumber(string number)
        {
            Number = number;
        }

        public DateOfBirth GetDateOfBirth()
        {
            int year = Convert.ToInt32(Number.Substring(0, 2)) + 1900;
            int month = Convert.ToInt32(Number.Substring(2, 2));
            int day = Convert.ToInt32(Number.Substring(4, 2));

            return new DateOfBirth(new DateTime(year, month, day));
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IdentityNumber);
        }

        public virtual bool Equals(IdentityNumber other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Number.Equals(Number);
            }

            return false;
        }

        public static bool operator ==(IdentityNumber left, IdentityNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IdentityNumber left, IdentityNumber right)
        {
            return !Equals(left, right);
        }
    }
}