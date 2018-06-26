using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Carroll.Data.Services.Startup))]
namespace Carroll.Data.Services
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Audience = ConfigurationManager.AppSettings["ida:Audience"],
                    Tenant = ConfigurationManager.AppSettings["ida:Tenant"]

                });
            //var tokenValidationParameter = new System.IdentityModel.Tokens.TokenValidationParameters();
            //tokenValidationParameter.ValidAudience = ConfigurationManager.AppSettings["ida:Audience"];


            //app.UseWindowsAzureActiveDirectoryBearerAuthentication(
            //new WindowsAzureActiveDirectoryBearerAuthenticationOptions
            //{
            //    TokenValidationParameters = tokenValidationParameter,
            //    Tenant = ConfigurationManager.AppSettings["ida:Tenant"]

            //});




        }
    }
}
