using System;
using Domain.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shouldly;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Policies_with_the_same_policy_number_are_equal()
        {
            var first = new Policy(new PolicyNumber("0123456789"));
            var second = new Policy(new PolicyNumber("0123456789"));

            (first.PolicyNumber == second.PolicyNumber).ShouldBe(true);
            first.PolicyNumber.Equals(second.PolicyNumber).ShouldBe(true);
        }

        /* Policy Creation(PlanCode xxx, MaimMember, Payour, premium, ) 
         * 
         */
    }

    public class Policy
    {
        public PolicyNumber PolicyNumber { get; set; }
        
        public Policy(PolicyNumber policyNumber)
        {
            Mandate.ParameterNotNull(policyNumber, "policyNumber");

            PolicyNumber = policyNumber;
        }
    }
}
