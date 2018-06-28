using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Carroll.Portals.Helpers
{
    public class MyExceptionHandler : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContextBase ctx = filterContext.HttpContext;

            // if the request is AJAX return JSON else view.
            //if (filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    filterContext.HttpContext.Response.StatusCode = 500;
            //    //filterContext.Result = new JsonResult
            //    //{
            //    //    Data = new
            //    //    {
            //    //        // obviously here you could include whatever information you want about the exception
            //    //        // for example if you have some custom exceptions you could test
            //    //        // the type of the actual exception and extract additional data
            //    //        // For the sake of simplicity let's suppose that we want to
            //    //        // send only the exception message to the client
            //    //        ErrorId = log.id,
            //    //        errorMessage = filterContext.Exception.Message
            //    //    },
            //    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //    //};

            //}

            // check if session is supported
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

            base.OnActionExecuting(filterContext);
        }
    }
}