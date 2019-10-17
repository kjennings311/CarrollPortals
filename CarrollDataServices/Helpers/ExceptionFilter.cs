using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;


namespace Carroll.Data.Services.Helpers
{
    public class ExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            StringBuilder sb = new StringBuilder();
            Exception e = filterContext.Exception;

            if (e.GetType() == typeof(DbEntityValidationException))
            {
                var innerException = e as DbEntityValidationException;
                if (innerException != null)
                {
                    sb.AppendLine();
                    sb.AppendLine();
                    foreach (var eve in innerException.EntityValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage));
                        }
                    }
                    sb.AppendLine();
                }
            }
            else
            {
                if (e.InnerException != null)
                {
                    sb.AppendLine();
                    sb.AppendLine();

                    if (e.InnerException != null)
                    {
                        sb.Append(e.InnerException.Message);
                    }

                    //foreach (var eve in e.InnerException.Message)
                    //{

                    //    sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //        eve.Entry.Entity.GetType().FullName, eve.Entry.State));

                    //    foreach (var ve in eve.ValidationErrors)
                    //    {
                    //        sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                    //            ve.PropertyName,
                    //            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                    //            ve.ErrorMessage));
                    //    }
                    //}
                    sb.AppendLine();
                }
            }
            //    Exception e = filterContext.Exception;
            //Get a StackTrace object for the exception
            StackTrace st = new StackTrace(e, true);

            //Get the first stack frame
            StackFrame frame = st.GetFrame(0);

            //Get the file name
            string fileName = frame.GetFileName();

            //Get the method name
            string methodName = frame.GetMethod().Name;

            //Get the line number from the stack frame
            int line = frame.GetFileLineNumber();

            //Get the column number
            int col = frame.GetFileColumnNumber();

            //var 

            //log.action = (string)filterContext.RouteData.Values["action"];
            //log.controller = (string)filterContext.RouteData.Values["controller"];
            //log.dev_status = 0;
            //log.message = sb.ToString() + " " + e.Message + line.ToString();
            //log.type = e.GetType().ToString();
            //log.raiseddatetime = DateTime.Now;
            //log.url = filterContext.HttpContext.Request.RawUrl;
            //log.source = e.Source;

            //if (filterContext.HttpContext.Session["Vm_UserId"] != null)
            //{
            //    log.userid = Convert.ToInt16(filterContext.HttpContext.Session["Vm_UserId"]);
            //}
            SiteUser user;
            if (filterContext.HttpContext.Session["user"] != null)
            {
                 user = (SiteUser)filterContext.HttpContext.Session["user"];

            }
            else
            {
                user = new SiteUser();
                //if (!string.IsNullOrEmpty(filterContext.HttpContext.Request.Params["CreatedByName"].ToString()))
                //    user.FirstName = filterContext.HttpContext.Request.Params["CreatedByName"].ToString();

            }


            ErrorLog errorLog = new ErrorLog();
            errorLog.datetime = DateTime.Now;
            errorLog.UserName = user.FirstName + " " + user.LastName;
            errorLog.Page = (string)filterContext.RouteData.Values["controller"] + "/" + (string)filterContext.RouteData.Values["action"];
            errorLog.Error = e.GetType().ToString()+" at"+ filterContext.HttpContext.Request.RawUrl;
            errorLog.Description = sb.ToString() + " " + e.Message + line.ToString();

            var rep = new EntityDataRepository();
            rep.ErrorLog(errorLog);

            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.ExceptionHandled = true;
            filterContext.Result = new JsonResult
            {
                Data = new
                {
                    // obviously here you could include whatever information you want about the exception
                    // for example if you have some custom exceptions you could test
                    // the type of the actual exception and extract additional data
                    // For the sake of simplicity let's suppose that we want to
                    // send only the exception message to the client
                    ErrorId = errorLog.LogId,
                    exceptionMessage = errorLog.Error
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };


        }
    }
}