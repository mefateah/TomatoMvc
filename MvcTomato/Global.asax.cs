﻿using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Optimization;

namespace MvcTomato
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            DatabaseConfig.Register();
            
            AreaRegistration.RegisterAllAreas();
            // TODO: what is that?
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}