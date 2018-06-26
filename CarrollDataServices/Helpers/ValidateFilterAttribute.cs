using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Filters;
using System.Net.Http;
using Carroll.Data.Services.Models.Validation;
using System.Web.Http.ModelBinding;

namespace ContactManagerLibrary.Helpers
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
      
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {

            if (!actionExecutedContext.ActionContext.ModelState.IsValid)
            {
             
                var errors = actionExecutedContext.ActionContext.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
                actionExecutedContext.ActionContext.Response =
                    actionExecutedContext.ActionContext.Request.CreateResponse(HttpStatusCode.BadRequest, new { errors });
            }
            base.OnActionExecuted(actionExecutedContext);
        }
      
        
    }
}
