using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Carroll.Portals.Helpers
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{

        //    // If they are authorized, handle accordingly
        //    if (this.AuthorizeCore(filterContext.HttpContext))
        //    {
        //        base.OnAuthorization(filterContext);
        //    }
        //    else
        //    {
        //        // Otherwise redirect to your specific authorized area
        //        filterContext.Result = new RedirectResult("~/Account/Login");
        //    }
        //}

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        errorMessage = "Sorry, your session has expired. Please login again to continue",
                        LogOnUrl = FormsAuthentication.LoginUrl
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };


                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }
            else
            {
                // this is a standard request, let parent filter to handle it
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }

   
}