using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using System.Web.Http.Filters;

namespace Carroll.Data.Services.Helpers
{
    public class AuthorizeRolesAttribute : AuthorizationFilterAttribute
    {
        private readonly string[] allowedroles;
        public AuthorizeRolesAttribute(params string[] roles):base()
        {
            this.allowedroles = roles;
        }
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
           
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            if (principal.Identity.IsAuthenticated)
            {
                var userName = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Not Authorized! Please contact your IT Department");
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Not Authorized! Please contact your IT Department");
            }
          //  return;
            //actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='eLearning' location='http://localhost:8323/account/login'");
        }
     

    }
}

//Usage:
//[AuthorizeRoles(Role.Administrator, Role.Assistant)]
//public ActionResult AdminOrAssistant()
//{
//    return View();
//}