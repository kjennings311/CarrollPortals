using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Carroll.Portals.Startup))]
namespace Carroll.Portals
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
