using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcTomato.Startup))]
namespace MvcTomato
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}