using Domain.Core;
using System;
using System.Runtime.Serialization;

namespace Domain.Client.ValueObjects
{
    [DataContract]
    public struct IdentityNumber
    {
        [DataMember(Order = 1, Name = "Number", IsRequired = true)]
        public string Number { get; private set; }

        public IdentityNumber(string number) : this()
        {
            Mandate.ParameterNotNullOrEmpty(number, "number");
            Mandate.ParameterCondition(number.Length >= 13, "number", "An identity number must be 13 digits long.");

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
            if (obj is IdentityNumber)
            {
                return Equals((IdentityNumber)obj);
            }

            return false;
        }

        public bool Equals(IdentityNumber other)
        {
            return other.Number.Equals(Number);
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