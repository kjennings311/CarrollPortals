using Carroll.Portals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carroll.Portals.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class MyIgnoreAttribute : Attribute
    {
    }


    public class BaseModel : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult v = filterContext.Result as ViewResult;
            if (!HasMyIgnoreAttribute(filterContext.ActionDescriptor))
            {

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
                        bm.AllowedProp = LoggedInUser.ReturnAllowedProperties();
                        bm.Isallowed = false;
                        if (Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "administrator" && Carroll.Portals.Models.LoggedInUser.AssignedRole().ToLower() != "hr")
                        {
                            bool isallowed = false;

                            var prop = Carroll.Portals.Models.LoggedInUser.AssignedUserProperty();
                            var AllowedProp = Carroll.Portals.Models.LoggedInUser.ReturnAllowedProperties();
                            string[] proplist = { };

                            if (prop.Contains("se"))
                            {
                                proplist = prop.Split(new string[] { "se" },StringSplitOptions.RemoveEmptyEntries );

                                foreach (string item in proplist)
                                {
                                    if(item !="")
                                    {
                                        if (AllowedProp.Contains(item))
                                        {
                                            isallowed = true;
                                            break;
                                        }
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
                            bm.Isallowed = isallowed;
                        }
                    }
                }
                base.OnActionExecuted(filterContext);
            }
            else
            {
                base.OnActionExecuted(filterContext);
            }
        }

        public bool HasMyIgnoreAttribute(ActionDescriptor actionDescriptor)
        {
            // Check if the attribute exists on the action method
            bool existsOnMethod = actionDescriptor.IsDefined(typeof(MyIgnoreAttribute), false);

            if (existsOnMethod)
            {
                return true;
            }

            // Check if the attribute exists on the controller
            return actionDescriptor.ControllerDescriptor.IsDefined(typeof(MyIgnoreAttribute), false);
        }
    }
}