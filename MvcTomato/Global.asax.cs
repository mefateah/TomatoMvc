using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace MvcTomato
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            DatabaseConfig.Register();
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}