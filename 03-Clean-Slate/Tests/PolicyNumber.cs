using Domain.Core;

namespace Tests
{
    public class PolicyNumber
    {
        public string Number { get; protected set; }

        public PolicyNumber(string number)
        {
            Mandate.ParameterNotNullOrEmpty(number, "policyNumber");
            Mandate.That(number.Length == 10);

            Number = number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var identity = obj as PolicyNumber;

            return identity != null && Equals(identity);
        }

        protected bool Equals(PolicyNumber other)
        {
            return string.Equals(Number, other.Number);
        }

        public override int GetHashCode()
        {
            return (Number != null ? Number.GetHashCode() : 0);
        }

        public static bool operator ==(PolicyNumber left, PolicyNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PolicyNumber left, PolicyNumber right)
        {
            return !Equals(left, right);
        }
    }
}