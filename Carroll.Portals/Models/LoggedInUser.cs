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

        public static string AssignedUserProperty()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[DateTime.Now.ToShortDateString() + "tlsporp"];

            //  FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authCookie != null)
            {
                string[] roles = authCookie.Value.Split(new Char[] { '|' });
                if (roles != null) return roles[0].ToString();
            }
               

           // if (roles != null && roles.Length == 2) return roles[1];
            return string.Empty;
        }

        public static string ReturnAllowedProperties()
        {
            // var str = "EF14E853-8958-413C-9B65-319EDE91A1A1seECF41C2B-AE15-46AF-9A66-E7D40FFCCE1EseAFD85A1D-5BA1-4783-9BA4-6DD8FD3436EEse2C86BF5B-9B9A-423D-93CB-B5AC5449A7D3se79789672-54D7-4DD7-9917-408193183FFCse7AC9B1B9-C050-4256-B54C-D57A3F43CD48se  227396E7-F998-4C6C-813D-17ED3FFBC39Bse647F767F-EB03-4BBC-87FA-36950421E08Bse80C63CE0-48DA-4B6D-AF8D-5DC492550E56se82D91DE3-5C9A-465D-8865-67A500B31169se96C786D5-E95D-4B0A-94CD-B30C69AF6480seBF71544E-EF1A-4990-AF9D-C0316343939CseEEF25F29-2C37-49D5-9B75-D5DAB443B496";
            var str = "7AC9B1B9-C050-4256-B54C-D57A3F43CD48se227396E7-F998-4C6C-813D-17ED3FFBC39Bse647F767F-EB03-4BBC-87FA-36950421E08Bse80C63CE0-48DA-4B6D-AF8D-5DC492550E56se82D91DE3-5C9A-465D-8865-67A500B31169se96C786D5-E95D-4B0A-94CD-B30C69AF6480seBF71544E-EF1A-4990-AF9D-C0316343939CseEEF25F29-2C37-49D5-9B75-D5DAB443B496";

            return str.ToLower();
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
                        String.Join("|", roles[0]),
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


                        // Cookie for storing user associated properties list

                   //     FormsAuthenticationTicket propticket = new FormsAuthenticationTicket(
                   //ident.Ticket.Version,
                   //ident.Ticket.Name,
                   //ident.Ticket.IssueDate,
                   //ident.Ticket.Expiration,
                   //ident.Ticket.IsPersistent,
                   //String.Join("|", roles[1]),
                   //ident.Ticket.CookiePath);

                        HttpCookie propcookie = new HttpCookie(DateTime.Now.ToShortDateString()+"tlsporp",
                                    roles[1]);
                        propcookie.Path = FormsAuthentication.FormsCookiePath + "; HttpOnly; noScriptAccess";
                        propcookie.Secure = FormsAuthentication.RequireSSL;

                        if (newticket.IsPersistent)
                            propcookie.Expires = newticket.Expiration;

                        HttpContext.Current.Response.Cookies.Add(propcookie);

                    }

                    // Create principal and attach to user
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(ident, roles);
                }
            }
            catch { }
        }
    }
}