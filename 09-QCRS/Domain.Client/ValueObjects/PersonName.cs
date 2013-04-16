using Domain.Core;
using System;
using System.Runtime.Serialization;

namespace Domain.Client.ValueObjects
{
    [DataContract]
    public struct PersonName : IEquatable<PersonName>
    {
        [DataMember(Order = 1, Name = "FirstName", IsRequired = true)]
        public string FirstName { get; private set; }

        [DataMember(Order = 2, Name = "Surname", IsRequired = true)]
        public string Surname { get; private set; }

        public PersonName(string firstName, string surname) : this()
        {
            Mandate.ParameterNotNullOrEmpty(firstName, "firstName");
            Mandate.ParameterNotNullOrEmpty(surname, "surname");

            FirstName = firstName;
            Surname = surname;
        }

        public override int GetHashCode()
        {
            return FirstName.GetHashCode() ^ Surname.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is PersonName && Equals((PersonName)obj);
        }

        public bool Equals(PersonName other)
        {
            return other.FirstName == FirstName
                && other.Surname == Surname;
        }

        public static bool operator ==(PersonName left, PersonName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PersonName left, PersonName right)
        {
            return !Equals(left, right);
        }
    }
}