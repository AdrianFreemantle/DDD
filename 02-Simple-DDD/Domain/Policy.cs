using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

using Infrastructure;

namespace Domain
{
    [DataContract]
    public class Policy : IGlobalIdentity
    {
        private const decimal PremiumIncreasePercentage = 10;
        private const decimal PremiumPercentageOfCover = 1;

        [DataMember]public Guid Id { get; protected set; }
        [DataMember]public string PolicyNumber { get; protected set; }
        [DataMember]public decimal SumAssured { get; protected set; }
        [DataMember]public decimal Premium { get; protected set; }
        [DataMember]public DateTime CaptureDate { get; protected set; }
        [DataMember]public DateTime DateOfCommencement { get; protected set; }
        [DataMember]public bool IsActive { get; protected set; }
        [DataMember]public string PlanDescription { get; protected set; }
        [DataMember]public string PlanCode { get; protected set; }
        [DataMember]public string MasterContractNumber { get; protected set; }

        protected Policy()
        {
            
        }

        public static Policy CreatePolicy(Guid policyId, string masterContractNumber, decimal coverAmount)
        {
            if (coverAmount <= 0)
                throw new DomainFault("A policy's cover must be a positive amount");

            if (String.IsNullOrWhiteSpace(masterContractNumber) 
                || masterContractNumber.Length != 4 
                || !masterContractNumber.ToCharArray().All(Char.IsDigit))
                throw new DomainFault("A master contract number must consist of four digits");

            return new Policy
            {
                Id = policyId,
                CaptureDate = DateTime.Now.Date,
                DateOfCommencement = GetFirstDayOfNextMonth(),
                IsActive = true,
                MasterContractNumber = masterContractNumber,
                SumAssured = coverAmount,
                PlanCode = "PL",
                PlanDescription = "Premium Life",
                PolicyNumber = CreatePolicyNumber(),
                Premium = coverAmount * (PremiumPercentageOfCover / 100)
            };
        }

        private static DateTime GetFirstDayOfNextMonth()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
        }

        private static string CreatePolicyNumber()
        {
            return DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).Substring(10);
        }

        public void IncreasePremium()
        {
            if (!IsActive)
                throw new DomainFault("An inactive policy may not have its premium increased.");

            Premium = Premium * (1 + (PremiumIncreasePercentage / 100));
        }

        public void Inactivate()
        {
            IsActive = false;
            Premium = 0;
            SumAssured = 0;
        }

        public void IncreaseCover(decimal coverAmount)
        {
            if(!IsActive)
                throw new DomainFault("An inactive policy may not have its cover increased.");

            SumAssured = coverAmount;
        }
    }
}
