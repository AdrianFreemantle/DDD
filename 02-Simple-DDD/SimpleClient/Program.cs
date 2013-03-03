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
            var policyId = new Guid();

            try
            {
                policyApplicationService.CreatePolicy(policyId, "0011", 50000);
                PrintPolicyDetails(policyId);

                policyApplicationService.IncreasePremium(policyId);
                PrintPolicyDetails(policyId);

                policyApplicationService.Inactivate(policyId);
                PrintPolicyDetails(policyId);

                //we expect this to throw a domain exception.
                policyApplicationService.IncreaseCover(policyId, 5000);
                PrintPolicyDetails(policyId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured : {0}", ex.Message);
            }

            Console.ReadKey();
        }

        static void PrintPolicyDetails(Guid policyId)
        {
            var policy = policyApplicationService.GetPolicy(policyId);

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
