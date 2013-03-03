using System.Linq;
using System.Web.Mvc;

using Domain;

namespace WebClient.Controllers
{ 
    public class PolicyController : Controller
    {
        public ViewResult Index()
        {
            return View(MvcApplication.Repository.GetQuery<Policy>().ToList());
        }

        public ViewResult Details(int id)
        {
            Policy policy = MvcApplication.Repository.Get<Policy>(id);
            return View(policy);
        }

        public ActionResult Edit(int id)
        {
            Policy policy = MvcApplication.Repository.Get<Policy>(id);
            return View(policy);
        }

        [HttpPost]
        public ActionResult Edit(Policy policy)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(policy);
        }
    }
}