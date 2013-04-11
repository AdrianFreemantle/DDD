using System;
using System.Runtime.Serialization;
using Domain.Core;

namespace Domain.Client.ValueObjects
{
    [DataContract]
    public struct BillingResult : IEquatable<BillingResult>
    {
        [DataMember(Order = 1, Name = "Paid", IsRequired = true)]
        public bool Paid { get; private set; }

        [DataMember(Order = 2, Name = "Amount", IsRequired = true)]
        public decimal Amount { get; private set; }

        [DataMember(Order = 3, Name = "PaymentDate", IsRequired = true)]
        public DateTime PaymentDate { get; private set; }

        public static BillingResult PaymentMade(decimal amount, DateTime paymentDate)
        {
            Mandate.ParameterCondition(amount > 0, "amount", "Must be a positive amount.");
            Mandate.ParameterCondition(paymentDate < DateTime.Today.AddDays(1), "paymentDate", "The payment date cannot be in the future.");

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
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is BillingResult && Equals((BillingResult)obj);
        }

        public bool Equals(BillingResult other)
        {
                return other.Paid == Paid
                    && other.Amount == Amount
                    && other.PaymentDate == PaymentDate;        
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