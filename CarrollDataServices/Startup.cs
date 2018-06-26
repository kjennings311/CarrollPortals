using Carroll.Data.Services.Helpers;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Carroll.Data.Services.Startup))]
namespace Carroll.Data.Services
{
    
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.Use<LoggingMiddleware>();
            //ConfigureAuth(app);
        }
    }
}
