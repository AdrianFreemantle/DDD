using Domain.Core;
using System;
using System.Runtime.Serialization;

namespace Domain.Client.ValueObjects
{
    [DataContract]
    public struct DateOfBirth
    {
        [DataMember(Order = 1, Name = "Date", IsRequired = true)]
        public DateTime Date { get; private set; }

        public DateOfBirth(DateTime dateOfBith) : this()
        {
            Mandate.ParameterCondition(dateOfBith < DateTime.Today.AddDays(1), "dateOfBith", "The date of birth cannot be in the future.");

            Date = dateOfBith.Date;
        }

        public PersonAge GetAgeAtDate(DateTime date)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - Date.Year;

            if (Date > today.AddYears(-age))
            {
                age--;
            }

            return new PersonAge(age);
        }

        public PersonAge GetCurrentAge()
        {
            return GetAgeAtDate(DateTime.Now);
        }

        public static implicit operator DateTime(DateOfBirth dateOfBirth)
        {
            return dateOfBirth.Date;
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is DateOfBirth)
            {
                return Equals((DateOfBirth)obj);
            }

            return false;
        }

        public bool Equals(DateOfBirth other)
        {
            return other.Date == Date;
        }

        public static bool operator ==(DateOfBirth left, DateOfBirth right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DateOfBirth left, DateOfBirth right)
        {
            return !Equals(left, right);
        }
    }
}