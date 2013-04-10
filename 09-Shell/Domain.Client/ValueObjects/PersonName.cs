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
            if (obj is PersonName)
            {
                return Equals((PersonName)obj);
            }

            return false;
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