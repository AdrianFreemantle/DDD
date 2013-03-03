using System;

using ApplicationServices;

using Domain;

using Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Should;

namespace Tests
{
    [TestClass]
    public class PolicyServiceTests
    {
        const int defaultPolicyId = 1;
        const decimal defaultCoverAmount = 10000;
        const string defaultMasterContract = "1234";

        PolicyApplicationService policyService;
        Policy defaultTestPolicy;

        [TestInitialize]
        public void TestInit()
        {
            policyService = new PolicyApplicationService(new UnitOfWork(new InMemoryRepository()));

            //add default policy for testing
            defaultTestPolicy = policyService.CreatePolicy(defaultMasterContract, defaultCoverAmount);
        }

        [TestMethod]
        public void A_new_policy_has_valid_initial_values()
        {
            var defaultPolicy = policyService.GetPolicy(defaultPolicyId);

            defaultPolicy.CaptureDate.ShouldEqual(DateTime.Now.Date);
            defaultPolicy.IsActive.ShouldBeTrue();
            defaultPolicy.MasterContractNumber.ShouldEqual(defaultMasterContract);
            defaultPolicy.SumAssured.ShouldEqual(defaultCoverAmount);
        }

        [TestMethod]
        public void A_deactivated_policy_has_zero_premium_and_cover_zero_premium_and_cover()
        {
            var policy = policyService.Inactivate(defaultPolicyId);

            policy.IsActive.ShouldBeFalse();
            policy.SumAssured.ShouldEqual(0);
            policy.Premium.ShouldEqual(0);
        }

        [TestMethod]
        public void Can_increase_cover_on_an_active_policy()
        {
            const decimal increaseAmount = 5000;
            var policy = policyService.IncreaseCover(defaultTestPolicy.Id, increaseAmount);

            policy.SumAssured.ShouldEqual(increaseAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Can_only_increase_cover_on_an_active_policy()
        {
            defaultTestPolicy.IsActive = false;
            policyService.IncreaseCover(defaultTestPolicy.Id, 5000);
        }
    }
}
