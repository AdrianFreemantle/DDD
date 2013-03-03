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

        public ViewResult Details(int id)
        {
            Policy policy = policyService.GetPolicy(id);
            return View(policy);
        }
        
        //Deliberately disabled post filters to allow us to call these actions by editing the url
        //[HttpPost]
        public ActionResult IncreasePremium(int id)
        {
            var policy = policyService.IncreasePremium(id);
            return View("Details", policy);
        }

        //[HttpPost]
        public ActionResult Inactivate(int id)
        {
            var policy = policyService.Inactivate(id);
            return View("Details", policy);
        }

        //[HttpPost]
        public ActionResult IncreaseCover(int id, decimal coverAmount)
        {
            var policy = policyService.IncreaseCover(id, coverAmount);
            return View("Details", policy);
        }
    }
}