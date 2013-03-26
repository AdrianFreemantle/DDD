using System;

namespace Tests.ValueObjects
{
    public class PersonName : IEquatable<PersonName>
    {
        public string FirstName { get; private set; }
        public string Surname { get; private set; }

        public PersonName(string firstName, string surname)
        {
            FirstName = firstName;
            Surname = surname;
        }

        public override int GetHashCode()
        {
            return FirstName.GetHashCode() ^ Surname.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PersonName);
        }

        public virtual bool Equals(PersonName other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.FirstName == FirstName
                    && other.Surname == Surname;
            }

            return false;
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