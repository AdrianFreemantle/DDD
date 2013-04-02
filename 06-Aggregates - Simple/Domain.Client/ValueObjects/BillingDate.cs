using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Client.ValueObjects
{
    public enum SalaryPaymentType
    {
        Unknown,
        Weekly,
        Monthly,
        Fortnightly
    }

    public class BillingDate
    {
        public SalaryPaymentType SalaryPaymentType { get; private set; }

        public BillingDate(SalaryPaymentType value)
        {
            SalaryPaymentType = value;
        }

        public DateTime NextBillingDate()
        {
            return DateTime.Now;
        }

        public override int GetHashCode()
        {
            return SalaryPaymentType.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BillingDate);
        }

        public virtual bool Equals(BillingDate other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.SalaryPaymentType == SalaryPaymentType;
            }

            return false;
        }

        public static bool operator ==(BillingDate left, BillingDate right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BillingDate left, BillingDate right)
        {
            return !Equals(left, right);
        }
    }
}
