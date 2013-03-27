namespace Tests.ValueObjects
{
    public class PersonAge
    {
        public int Age { get; set; }

        public PersonAge(int age)
        {
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
            return Equals(obj as PersonAge);
        }

        public virtual bool Equals(PersonAge other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Age == Age;
            }

            return false;
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