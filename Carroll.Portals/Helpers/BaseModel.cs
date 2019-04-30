using Carroll.Portals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carroll.Portals.Helpers
{
    public class BaseModel : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult v = filterContext.Result as ViewResult;

            if (v != null)
            {
                //if (v.Model == null)
                //    v.Model = (object)BaseViewModel();
                BaseViewModel bm = v.Model as BaseViewModel;

                if (bm != null)
                {
                    //HttpContextBase ctc = filterContext.HttpContext;
                    //bm.UserId = ctc.Session["Carroll_User_Id"].ToString();
                    //bm.UserName = ctc.Session["Carroll_UserName"].ToString();
                    //bm.RoleType = ctc.Session["Carroll_RoleName"].ToString();

                    bm.UserId = LoggedInUser.Current.UserId.ToString();
                    bm.UserName = LoggedInUser.Current.UserEmail;
                    bm.Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(LoggedInUser.Current.FirstName.ToLower()) + " " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(LoggedInUser.Current.LastName.ToLower());
                    bm.RoleType = LoggedInUser.AssignedRole();
                    bm.PropertyId = LoggedInUser.AssignedUserProperty();
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}