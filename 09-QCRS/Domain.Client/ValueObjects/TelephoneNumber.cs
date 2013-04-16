using Domain.Core;
using System;
using System.Runtime.Serialization;

namespace Domain.Client.ValueObjects
{
    [DataContract]
    public struct TelephoneNumber : IEquatable<TelephoneNumber>
    {
        [DataMember(Order = 1, Name = "Number", IsRequired = true)]
        public string Number { get; private set; }

        public TelephoneNumber(string number) : this()
        {
            Mandate.ParameterNotNullOrEmpty(number, "number");
            Mandate.ParameterCondition(number.Length >= 10, "number", "A telephone number must be 10 digits long.");

            Number = number;
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

            return obj is TelephoneNumber && Equals((TelephoneNumber)obj);
        }

        public bool Equals(TelephoneNumber other)
        {
            return other.Number == Number;
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