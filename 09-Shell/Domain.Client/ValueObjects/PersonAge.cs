using System;
using Domain.Core;
using System.Runtime.Serialization;
namespace Domain.Client.ValueObjects
{
    [DataContract]
    public struct PersonAge : IEquatable<PersonAge>
    {
        [DataMember(Order = 1, Name = "Age", IsRequired = true)]
        public int Age { get; set; }

        public PersonAge(int age) : this()
        {
            Mandate.ParameterCondition(age >= 0, "age", "A persons age must be a non negative number");

            Age = age;
        }

        public static implicit operator int(PersonAge age)
        {
            return age.Age;
        }

        public override int GetHashCode()
        {
            return Age;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is PersonAge && Equals((PersonAge)obj);
        }

        public bool Equals(PersonAge other)
        {
            return other.Age == Age;
        }

        public static bool operator ==(PersonAge left, PersonAge right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PersonAge left, PersonAge right)
        {
            return !Equals(left, right);
        }
    }
}