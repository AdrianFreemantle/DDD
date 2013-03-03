using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using ApplicationServices;

using Infrastructure;

namespace WebClient
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static UnitOfWork UnitOfWork { get; set; }
        public static PolicyApplicationService PolicyService { get; set; }

        static MvcApplication()
        {
            StartupConfig();
        }

        /// <summary>
        /// We dont want to have to deal with the complexity of using databases at this stage.
        /// This basic setup allows us to have an in memory database of sorts without adding 
        /// </summary>
        private static void StartupConfig()
        {
            UnitOfWork = new UnitOfWork(new InMemoryRepository());
            PolicyService = new PolicyApplicationService(UnitOfWork);

            PolicyService.CreatePolicy("0011", 20000);
            PolicyService.CreatePolicy("0011", 1000);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }        
    }
}