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

    public class AdminHRFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            HttpContextBase ctx = filterContext.HttpContext;

            if (Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "administrator" && Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "regional" && Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "vp" && Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "rvp" && Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "property" && Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "hr")
            {
                filterContext.Result = new ContentResult() { Content = "You do not have permission to access this resource. Please contact your IT department." };
            }
            if (Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "administrator" && Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "hr")
            {
                bool isallowed = false;
                var role = Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower();
                var prop = Carroll.Portals.Models.LoggedInUser.AssignedUserProperty();
                var AllowedProp = Carroll.Portals.Models.LoggedInUser.ReturnAllowedProperties();
                string[] proplist = { };

                if (prop.Contains("se"))
                {
                    proplist = prop.Split(new string[] { "se" },StringSplitOptions.RemoveEmptyEntries);

                    foreach (string item in proplist)
                    {
                        if(item != "")
                        if (AllowedProp.Contains(item))
                        {
                            isallowed = true;
                            break;
                        }

                    }
                }
                else
                {
                    if (AllowedProp.Contains(prop))
                    {
                        isallowed = true;

                    }
                }
     
            if (isallowed == false)
                filterContext.Result = new ContentResult() { Content = "You do not have permission to access this resource. Please contact your IT department." };
            }
            //}
            //if (!(filterContext.HttpContext.Session["Carroll_RoleName"].ToString().Contains("Administrator")))
            //{
            //    filterContext.Result = new ContentResult() { Content = "UnAuthorised Access to Specified Resource" };
            //}
        }

    }
}