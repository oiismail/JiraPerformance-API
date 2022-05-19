using JiraAPI;
using JiraAPI.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace JiraPerformanceAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new Container();
            container.Register<IJiraIssuesService>(() => new JiraIssuesService(), Lifestyle.Transient);
            container.Verify();
           // DependencyResolver.SetResolver(new simple)
        }
    }
}
