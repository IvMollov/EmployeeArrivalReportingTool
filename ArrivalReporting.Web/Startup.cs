using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArrivalReporting.Web.Startup))]
namespace ArrivalReporting.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
