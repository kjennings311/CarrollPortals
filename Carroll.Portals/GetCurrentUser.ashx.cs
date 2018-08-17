using Carroll.Data.Entities;
using Carroll.Portals.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carroll.Portals
{
    /// <summary>
    /// Summary description for GetCurrentUser
    /// </summary>
    public class GetCurrentUser : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            SiteUser _user = LoggedInUser.Current;
            context.Response.Write(JsonConvert.SerializeObject(_user));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}