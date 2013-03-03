using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Domain;

using Infrastructure;

namespace ApplicationServices
{
    /// <summary>
    /// In transaction script architectures, the invariants (rules) and behaviours of our domain are enforced
    /// in the kind of procedural "business logic" class such as the one implemented here. This style is
    /// very suitable to simpler domains and should always be considered as our first alternative to DDD when our 
    /// domain is not complex enough to warrent a full DDD implementation.   
    /// </summary>
    public class PolicyApplicationService
    {
        private readonly UnitOfWork unitOfWork;
        private const decimal PremiumIncreasePercentage = 10;
        private const decimal PremiumPercentageOfCover = 1;

        public PolicyApplicationService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Policy CreatePolicy(string masterContractNumber, decimal coverAmount)
        {
            //normally we would get much of the parameters below from a database lookup, factory or service.
            //we simplify it here for the sake of brevity and simply hardcode most values.

            try
            {
                var newPolicy = new Policy
                {
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

                unitOfWork.Repository.Add(newPolicy);
                ValidatePolicy(newPolicy);
                unitOfWork.Commit();
                return newPolicy;
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private static DateTime GetFirstDayOfNextMonth()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
        }

        private static string CreatePolicyNumber()
        {
            return DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).Substring(10);
        }

        public Policy IncreasePremium(int policyId)
        {
            try
            {
                var policy = unitOfWork.Repository.Get<Policy>(policyId);
                policy.Premium = policy.Premium * (1 + (PremiumIncreasePercentage / 100));

                ValidatePolicy(policy);
                unitOfWork.Commit();
                return policy;
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public Policy Inactivate(int policyId)
        {
            try
            {
                var policy = unitOfWork.Repository.Get<Policy>(policyId);
                policy.IsActive = false;
                policy.Premium = 0;
                policy.SumAssured = 0;

                ValidatePolicy(policy);
                unitOfWork.Commit();
                return policy;
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public Policy IncreaseCover(int policyId, decimal coverAmount)
        {
            try
            {
                var policy = unitOfWork.Repository.Get<Policy>(policyId);
                policy.SumAssured = coverAmount;

                //we "forget" to call validate, creating the potential for a serious violation of business rules.
                //ValidatePolicy(policy);
                unitOfWork.Commit();
                return policy;
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public IEnumerable<Policy> GetAllPolicies()
        {
            return unitOfWork.Repository.GetQuery<Policy>().ToList();
        }

        public Policy GetPolicy(int policyId)
        {
            return unitOfWork.Repository.Get<Policy>(policyId);
        }

        /// <summary>
        /// In transaction script architectures we often tend to see our validation concentrated in a sigle location such as this. 
        /// An alternate form uses Data Annotation validations on our domain model class combined with more complex validation being done in a
        /// method such as this one. In the end, we are mainly focussed on cecking that the state of the domain object to be persisted
        /// is valid just before doing the actual persistence.
        /// </summary>
        private void ValidatePolicy(Policy policy)
        {
            if (policy == null)
                throw new ArgumentNullException("policy");

            if (String.IsNullOrWhiteSpace(policy.PlanCode))
                throw new Exception("The policy plan code must contain a valid string.");

            if (policy.PlanCode.Length != 2)
                throw new Exception("The policy plan code must contain exactly two characters");

            if (String.IsNullOrWhiteSpace(policy.MasterContractNumber))
                throw new Exception("The policy Master Contract Number code must contain a valid string.");

            if (policy.MasterContractNumber.Length != 4 && policy.MasterContractNumber.ToCharArray().Any(c => !Char.IsDigit(c)))
                throw new Exception("The policy Master Contract Number must contain exactly four digits.");

            if (!policy.IsActive && policy.Premium > 0)
                throw new Exception("An inactive policy may not have a premium that is greater than 0.");

            if(!policy.IsActive && policy.SumAssured > 0)
                throw new Exception("An inactive policy may not provide a valid cover amount");
        }
    }
}
