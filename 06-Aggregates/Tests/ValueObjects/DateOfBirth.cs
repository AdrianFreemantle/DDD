using System;

namespace Tests.ValueObjects
{
    public class DateOfBirth
    {
        public DateTime Date { get; private set; }

        public DateOfBirth(DateTime dateOfBith)
        {
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
            return Equals(obj as DateOfBirth);
        }

        public virtual bool Equals(DateOfBirth other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Date == Date;
            }

            return false;
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