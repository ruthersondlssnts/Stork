using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_1ClickDelivery.Startup))]
namespace _1ClickDelivery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
