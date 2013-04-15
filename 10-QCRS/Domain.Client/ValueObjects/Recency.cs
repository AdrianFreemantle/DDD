using System;
using System.Runtime.Serialization;
namespace Domain.Client.ValueObjects
{
    [DataContract]
    public struct Recency : IEquatable<Recency>
    {
        [DataMember(Order = 1, Name = "Value", IsRequired = true)]
        public int Value { get; private set; }

        public Recency(int value) : this()
        {
            Value = value;
        }

        public static Recency UpToDate()
        {
            return new Recency(0);
        }

        public Recency IncreaseRecency()
        {
            return new Recency(Value + 1);
        }

        public Recency FromBillingResult(BillingResult billingResult)
        {
            return billingResult.Paid ? UpToDate() : IncreaseRecency();
        }

        public bool IsUpToDate()
        {
            return Value == 0;
        }

        public bool ShouldAccountBeLapsed()
        {
            return Value == 6;
        }

        public bool ShouldAccountBeSuspended()
        {
            return Value == 3;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Recency && Equals((Recency)obj);
        }

        public bool Equals(Recency other)
        {
            return other.Value == Value;
        }

        public static bool operator ==(Recency left, Recency right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Recency left, Recency right)
        {
            return !Equals(left, right);
        } 
    }
}