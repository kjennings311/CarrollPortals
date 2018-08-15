using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Carroll.Portals.Models
{
    public sealed class LoggedInUser
    {
         public static SiteUser Current
        {
            get
            {
                if (HttpContext.Current.Session["user"] == null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {

                        string userName = HttpContext.Current.User.Identity.Name;
                        EntityUserRepository _userservice = new EntityUserRepository();
                        HttpContext.Current.Session["user"] = _userservice.GetUser(userName);
                        AssignRolesToUser();
                    }
                }

                return (SiteUser)HttpContext.Current.Session["user"];
            }
            set { HttpContext.Current.Session["user"] = value; }
        }

        public static string AssignedRole()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket authTicket =  FormsAuthentication.Decrypt(authCookie.Value);
            string[] roles = authTicket.UserData.Split(new Char[] { '|' });
            if (roles != null) return roles[0];

            return string.Empty;
        }
        public static void AssignRolesToUser()
        {
            try
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    // Retrieve user's identity from context user
                    FormsIdentity ident = (FormsIdentity)HttpContext.Current.User.Identity;

                    // Retrieve roles from the authentication ticket userdata field
                    string[] roles = ident.Ticket.UserData.Split('|');

                    // If we didn't load the roles before, go to the DB
                    if (roles[0].Length == 0)
                    {
                        EntityUserRepository _userservice = new EntityUserRepository();
                        // Fetch roles from the database.
                        roles = _userservice.GetUserRoleName(_userservice.GetUser(HttpContext.Current.User.Identity.Name).UserId).Split(',');

                        // Store roles inside the Forms ticket.
                        FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(
                        ident.Ticket.Version,
                        ident.Ticket.Name,
                        ident.Ticket.IssueDate,
                        ident.Ticket.Expiration,
                        ident.Ticket.IsPersistent,
                        String.Join("|", roles),
                        ident.Ticket.CookiePath);

                        // Create the cookie.    
                        HttpCookie authCookie = new HttpCookie(
                        FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(newticket));
                        authCookie.Path = FormsAuthentication.FormsCookiePath + "; HttpOnly; noScriptAccess";
                        authCookie.Secure = FormsAuthentication.RequireSSL;

                        if (newticket.IsPersistent)
                            authCookie.Expires = newticket.Expiration;

                        HttpContext.Current.Response.Cookies.Add(authCookie);
                    }

                    // Create principal and attach to user
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(ident, roles);
                }
            }
            catch { }
        }
    }
}