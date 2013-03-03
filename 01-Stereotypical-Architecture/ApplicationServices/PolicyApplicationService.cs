using System;
using System.Linq;

using Domain;

using Infrastructure;

namespace ApplicationServices
{
    public class PolicyApplicationService
    {
        private readonly IRepository repository;
        private const decimal PremiumIncreasePercentage = 10;
        private const decimal PremiumPercentageOfCover = 1;

        public PolicyApplicationService(IRepository repository)
        {
            this.repository = repository;
        }

        public Policy CreatePolicy(string masterContractNumber, decimal coverAmount)
        {
            //normally we would get much of the detail below from a database lookup, a factory or a service.
            //we simplify it here for the sake of brevity and simply hardcode most values.

            var newPolicy = new Policy
            {
                Id = 1,
                CaptureDate = DateTime.Now,
                DateOfCommencement = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1),//first day of next month
                IsActive = true,
                MasterContractNumber = masterContractNumber,
                SumAssured = coverAmount,
                PlanCode = "PL",
                PlanDescription = "Premium Life",
                PolicyNumber = DateTime.Now.Ticks.ToString().Substring(10),
                Premium = coverAmount * (PremiumPercentageOfCover / 100)
            };

            ValidatePolicy(newPolicy);
            return repository.Add(newPolicy);
        }

        public Policy IncreasePremium(Policy policy)
        {
            policy.Premium = policy.Premium * (1 + (PremiumIncreasePercentage / 100));
            ValidatePolicy(policy);
            return repository.Update(policy);
        }

        public Policy Inactivate(Policy policy)
        {
            policy.IsActive = false;
            policy.Premium = 0;
            policy.SumAssured = 0;
            ValidatePolicy(policy);
            return repository.Update(policy);
        }

        public Policy IncreaseCover(Policy policy, decimal coverAmount)
        {
            policy.SumAssured = coverAmount;
            repository.Update(policy);
            return policy; //note that we forgot to call validate, creating the potential for a serious violation of business rules.
        }

        public Policy GetPolicy(int id)
        {
            return repository.Get<Policy>(id);
        }

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
