using System;

using ApplicationServices;

using Domain;

using Infrastructure;

namespace Client
{
    /// <summary>
    /// A very simple console application for the testing of our current application service.
    /// </summary>
    class Program
    {
        static readonly PolicyApplicationService policyApplicationService;

        static Program()
        {
            policyApplicationService = new PolicyApplicationService(new UnitOfWork(new InMemoryRepository()));
        }

        static void Main(string[] args)
        {
            var policy = policyApplicationService.CreatePolicy("0011", 50000);
            PrintPolicyDetails(policy);

            policy = policyApplicationService.IncreasePremium(policy.Id);
            PrintPolicyDetails(policy);

            policy = policyApplicationService.Inactivate(policy.Id);
            PrintPolicyDetails(policy);

            //this method call is breaking our business rule which says that you cant increase the cover of a policy which is inactive.
            policy = policyApplicationService.IncreaseCover(policy.Id, 5000);
            PrintPolicyDetails(policy);

            Console.ReadKey();
        }

        static void PrintPolicyDetails(Policy policy)
        {
            Console.WriteLine("{0} policy {1} is currently {2} and provides {3} cover with a montly premium of {4}", 
                policy.PlanDescription, 
                policy.PolicyNumber, 
                policy.IsActive ? "active" : "inactive",
                policy.SumAssured,
                policy.Premium);

            Console.WriteLine();
        }
    }
}
