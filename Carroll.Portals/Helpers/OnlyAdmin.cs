using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Carroll.Portals.Helpers
{
    public class OnlyAdminFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            HttpContextBase ctx = filterContext.HttpContext;
            if (ctx.Session["Vm_UserId"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            // obviously here you could include whatever information you want about the exception
                            // for example if you have some custom exceptions you could test
                            // the type of the actual exception and extract additional data
                            // For the sake of simplicity let's suppose that we want to
                            // send only the exception message to the client
                            ErrorId = 401,
                            errorMessage = "Session Expired. Plz Login Again"
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };

                }
                else
                {
                    FormsAuthentication.SignOut();
                    filterContext.Result = new RedirectResult("/Account/Login");
                }

            }
            else
                if (!(filterContext.HttpContext.Session["Vm_RoleName"].ToString() == "Admin" || filterContext.HttpContext.Session["Vm_RoleName"].ToString() == "Super User"))
            {
                filterContext.Result = new ContentResult() { Content = "UnAuthorised Access to Specified Resource" };

            }
        }

    }
}