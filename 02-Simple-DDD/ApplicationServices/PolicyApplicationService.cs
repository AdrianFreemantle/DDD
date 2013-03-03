using System;
using System.Collections.Generic;
using System.Linq;

using Domain;

using Infrastructure;

namespace ApplicationServices
{
    public class PolicyApplicationService
    {
        private readonly UnitOfWork unitOfWork;

        public PolicyApplicationService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreatePolicy(Guid policyId, string masterContractNumber, decimal coverAmount)
        {
            try
            {
                var policy = Policy.CreatePolicy(policyId, masterContractNumber, coverAmount);
                unitOfWork.Repository.Add(policy);
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public void IncreasePremium(Guid policyId)
        {
            try
            {
                var policy = unitOfWork.Repository.Get<Policy>(policyId);
                policy.IncreasePremium();
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public void Inactivate(Guid policyId)
        {
            try
            {
                var policy = unitOfWork.Repository.Get<Policy>(policyId);
                policy.Inactivate();
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public void IncreaseCover(Guid policyId, decimal coverAmount)
        {
            try
            {
                var policy = unitOfWork.Repository.Get<Policy>(policyId);
                policy.IncreaseCover(coverAmount);
                unitOfWork.Commit();
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

        public Policy GetPolicy(Guid policyId)
        {
            return unitOfWork.Repository.Get<Policy>(policyId);
        }
    }
}
