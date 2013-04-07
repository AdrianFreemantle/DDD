using System;
using System.Runtime.Serialization;

namespace Domain.Client.ValueObjects
{
    [DataContract]
    public class BillingResult 
    {
        [DataMember(Order = 0, Name = "Paid", IsRequired = true)]
        public bool Paid { get; private set; }

        [DataMember(Order = 1, Name = "Amount", IsRequired = true)]
        public decimal Amount { get; private set; }

        [DataMember(Order = 2, Name = "PaymentDate", IsRequired = true)]
        public DateTime PaymentDate { get; private set; }

        private BillingResult()
        {
        }

        public static BillingResult PaymentMade(decimal amount, DateTime paymentDate)
        {
            return new BillingResult
            {
                Amount = amount,
                Paid = true,
                PaymentDate = paymentDate.Date
            };
        }

        public static BillingResult NotPaid(DateTime paymentDate)
        {
            return new BillingResult
            {
                Amount = 0,
                Paid = false,
                PaymentDate = paymentDate.Date
            };
        }

        public override int GetHashCode()
        {
            return Paid.GetHashCode() ^ Amount.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BillingResult);
        }

        public virtual bool Equals(BillingResult other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Paid == Paid
                       && other.Amount == Amount
                       && other.PaymentDate == PaymentDate;
            }

            return false;
        }

        public static bool operator ==(BillingResult left, BillingResult right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BillingResult left, BillingResult right)
        {
            return !Equals(left, right);
        }
    }
}