using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Configuration;

namespace Carroll.Data.Services.Helpers
{
    public class CustomActionFilter: ActionFilterAttribute
    {
       

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
           
        }
      

       
    }
}

//var task = Task.Run(async () => {

//    return await engine.DoAsyncFunction();

//});

//var Result = task.Result;