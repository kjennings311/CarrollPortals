using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Carroll.Portals.Helpers
{
    public class AdminFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            HttpContextBase ctx = filterContext.HttpContext;
           
            if(Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "administrator")
            {
                filterContext.Result = new ContentResult() { Content = "You do not have permission to access this resource. Please contact your IT department." };
            }
            //if (!(filterContext.HttpContext.Session["Carroll_RoleName"].ToString().Contains("Administrator")))
            //{
            //    filterContext.Result = new ContentResult() { Content = "UnAuthorised Access to Specified Resource" };
            //}
        }

    }
}