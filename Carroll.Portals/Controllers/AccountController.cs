using Carroll.Data.Services.Models;
using Carroll.Data.Services.Controllers;
using System.Web.Mvc;

using Carroll.Portals.Models;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Entities;
using Carroll.Data.Services.Models.Validation;
using System.Web.Security;
using System.Collections.Generic;
using Carroll.Portals.Helpers;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Web;
using System;

namespace Carroll.Portals.Controllers
{
    public class AccountController : Controller
    {
      
        private EntityUserRepository _userservice;

        public AccountController()
        {
            _userservice = new EntityUserRepository();

        }


        [AllowAnonymous]
        [HttpGet]
        // GET: Account
        public ActionResult Login()
        {
            if(Session["Vm_UserId"] == null)

            return View();
            else
                return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: Account
        public ActionResult CheckLogin(LoginModel Login)
        {
            if (ModelState.IsValid)
            {
                // check user exist with service
                // if valid create cookie and session for this user.

                if (_userservice.AuthenticateUser(Login.UserName, Login.Password))
                {

                    // is a valid user, create cookie, create session variables
                    FormsAuthentication.SetAuthCookie(Login.UserName, true);
                    LoggedInUser.AssignRolesToUser();
                    // Get User Credentials
                    // SiteUser user = _userservice.GetUser(Login.UserName);

                    //string role = _userservice.GetUserRoleName(user.UserId);

                    // if (user != null)
                    // {
                    //     Session["Carroll_User_Id"] = user.UserId;
                    //     Session["Carroll_UserName"] = user.FirstName + " " + user.LastName;                    
                    //    Session["Carroll_RoleName"] = role;

                    // }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // show error message
                    ModelState.AddModelError("Error", "Invalid Username or Password");
                }
            }
            else
            {
                ModelState.AddModelError("error", "Please Enter All Details");
            }
            return View("Login");
}

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: Account
        public ActionResult SendPassword(string Email)
        {
            if (ModelState.IsValid)
            {
                // check user exist with service
                // if valid create cookie and session for this user.

                if (_userservice.CheckIfUserExists(Email))
                {

                    // Send Mail Logic

                    //using (MailMessage mail = new MailMessage(new MailAddress("From Name", emailsettings.Company), new MailAddress(toemail, item.CustomerName)))
                    //{


                    //    mail.Subject = "Your Order Proforma " + item.OrderFormNumber + " Date :" + item.OrderDate.ToShortDateString();


                    //    mail.Body = "<p> Dear Customer " + item.ContactName + ", Your Order details are </p>" +
                    //        "<table><tr><td> Order Ref Number : </td><td>" + item.OrderFormNumber + " </td></tr> " +
                    //        "<tr><td>Order Date :   </td> <td>    " + item.OrderDate.ToShortDateString() + "</td></tr>" +
                    //        " <tr><td> Amount :</td> <td>      " + item.NetTotalDue.ToString("#.##") + " </td></tr></table>";

                    //    string path = Server.MapPath(@"~/Content/img/" + emailsettings.logo);
                    //    LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                    //    Img.ContentId = "MyImage";

                    //    string header = "";
                    //    if (includecm)
                    //        header = mailstart + mail.Body + "<p>" + cms + " </p>" + mailfooterhtml;
                    //    else
                    //        header = mailstart + mail.Body + mailfooterhtml;



                    //    // string header = mailstart + "<p> Dear Customer " + item.ContactName + "  </p>  <br> ";


                    //    //     outstandingmodel.cms = outstandingmodel.cms.Replace("[Customer Name]", item.CustomerName + "<br>");

                    //    //  mail.Body = header + outstandingmodel.cms + mailfooterhtml;
                    //    //now do the HTML formatting

                    //    AlternateView av1 = AlternateView.CreateAlternateViewFromString(
                    //        header,
                    //          null, MediaTypeNames.Text.Html);

                    //    //now add the AlternateView
                    //    av1.LinkedResources.Add(Img);

                    //    //now append it to the body of the mail
                    //    mail.AlternateViews.Add(av1);


                    //    mail.IsBodyHtml = true;




                    //    SmtpClient smtp = new SmtpClient();
                    //    smtp.Host = "XXXXXXXX"; // smtp.Host = "smtp.gmail.com";
                    //    smtp.EnableSsl = true;
                    //    smtp.UseDefaultCredentials = false;
                    //    NetworkCredential networkCredential = new NetworkCredential("XXXXXXXX", "XXXXXXXXXX");

                    //    smtp.Credentials = networkCredential;
                    //    smtp.Port = 587; //587


                    //    mailsent = false;
                    //    mailmsg = "";
                    //    bool validemail = true;
                    //    try
                    //    {
                    //        //if (!string.IsNullOrEmpty(item.CompanyEmail))
                    //        //{

                    //        //    Match match = regex.Match(item.CompanyEmail);
                    //        //    if (match.Success)
                    //        //    {
                    //        //        mail.To.Add(item.CompanyEmail);
                    //        //        validemail = true;
                    //        //    }
                    //        //}


                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        //mailsent = false;
                    //        //mailmsg = ex.Message;

                    //        //error.ErrorList.Add(new Error { ErrorExist = true, ErrorType = " Mail Function Failure", ErrorMsg = ex.Message });

                    //    }
                    return RedirectToAction("Login", "Account");
                }

                    
              
                else
                {
                    // show error message
                    ModelState.AddModelError("Error", "No User Found with given Email Address");
                }
            }
            else
            {
                ModelState.AddModelError("error", "Please Enter Email Address");
            }
            return View("ForgotPassword");
        }

        [CustomAuthorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
    }
}