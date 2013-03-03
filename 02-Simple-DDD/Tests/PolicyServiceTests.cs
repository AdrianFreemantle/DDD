using System;

using ApplicationServices;

using Domain;

using Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shouldly;

namespace Tests
{
    [TestClass]
    public class PolicyTests
    {
        const decimal defaultCoverAmount = 10000;
        const string defaultMasterContract = "1234";

        [TestMethod]
        public void A_new_policy_has_valid_initial_values()
        {
            var policy = Policy.CreatePolicy(Guid.NewGuid(), defaultMasterContract, defaultCoverAmount);

            policy.CaptureDate.ShouldBe(DateTime.Now.Date);
            policy.IsActive.ShouldBe(true);
            policy.MasterContractNumber.ShouldBe(defaultMasterContract);
            policy.SumAssured.ShouldBe(defaultCoverAmount);
        }

        [TestMethod]
        public void A_deactivated_policy_has_zero_premium_and_cover_zero_premium_and_cover()
        {
            var policy = Policy.CreatePolicy(Guid.NewGuid(), defaultMasterContract, defaultCoverAmount);
            policy.Inactivate();

            policy.IsActive.ShouldBe(false);
            policy.SumAssured.ShouldBe(0);
            policy.Premium.ShouldBe(0);
        }

        [TestMethod]
        public void Can_increase_cover_on_an_active_policy()
        {
            const decimal increaseAmount = 5000;
            var policy = Policy.CreatePolicy(Guid.NewGuid(), defaultMasterContract, defaultCoverAmount);
            policy.IncreaseCover(increaseAmount);

            policy.SumAssured.ShouldBe(increaseAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainFault))]
        public void Can_only_increase_cover_on_an_active_policy()
        {
            var policy = Policy.CreatePolicy(Guid.NewGuid(), defaultMasterContract, defaultCoverAmount);
            policy.Inactivate();
            policy.IncreaseCover(5000);
        }
    }
}
