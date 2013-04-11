using Domain.Core;
using System;
using System.Runtime.Serialization;

namespace Domain.Client.ValueObjects
{
    [DataContract]
    public struct IdentityNumber : IEquatable<IdentityNumber>
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
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is IdentityNumber && Equals((IdentityNumber)obj);
        }

        public bool Equals(IdentityNumber other)
        {
            return other.Number == Number;
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