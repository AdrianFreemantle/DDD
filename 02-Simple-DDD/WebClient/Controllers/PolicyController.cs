using System;
using System.Linq;
using System.Web.Mvc;

using ApplicationServices;

using Domain;

namespace WebClient.Controllers
{ 
    public class PolicyController : Controller
    {
        readonly PolicyApplicationService policyService;

        public PolicyController()
        {
            policyService = MvcApplication.PolicyService;
        }

        public ViewResult Index()
        {
            return View(policyService.GetAllPolicies());
        }

        public ViewResult Details(Guid id)
        {
            Policy policy = policyService.GetPolicy(id);
            return View(policy);
        }
        
        //Deliberately disabled post filters to allow us to call these actions by editing the url
        //[HttpPost]
        public ActionResult IncreasePremium(Guid id)
        {
            policyService.IncreasePremium(id);
            return View("Details", policyService.GetPolicy(id));
        }

        //[HttpPost]
        public ActionResult Inactivate(Guid id)
        {
            policyService.Inactivate(id);
            return View("Details", policyService.GetPolicy(id));
        }

        //[HttpPost]
        public ActionResult IncreaseCover(Guid id, decimal coverAmount)
        {
            policyService.IncreaseCover(id, coverAmount);
            return View("Details", policyService.GetPolicy(id));
        }
    }
}