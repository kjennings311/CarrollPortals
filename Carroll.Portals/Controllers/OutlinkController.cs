using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carroll.Portals.Helpers;
using Carroll.Portals.Models;
using Newtonsoft.Json;

using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using Carroll.Portals;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using Rotativa;
using System.IO;
using Rotativa.Options;
using ClosedXML.Excel;
using System.Data;
using ClosedXML.Extensions;
using System.Reflection;
using Carroll.Data.Entities;
using Carroll.Data.Services.Helpers;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace Carroll.Portals.Controllers
{

    [ExceptionFilter]
    public class OutlinkController : Controller
    {

        string Baseurl = "";

        public OutlinkController()
        {
            Baseurl = ConfigurationManager.AppSettings["ServiceURL"];
        }


        // GET: Outlink
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Verify(string link)
        {
            // Check Link is open, if closed give message link expired
            var linkid = new Guid(link);

            using (var _entities = new CarrollFormsEntities())
            {
                var dlink = (from tbl in _entities.DynamicLinks
                             where tbl.DynamicLinkId == linkid
                             select tbl).FirstOrDefault();

                var newhire = (from tbl in _entities.EmployeeNewHireNotices
                               where tbl.EmployeeHireNoticeId == dlink.ReferenceId
                               select tbl).FirstOrDefault();

                if (dlink == null)
                {
                    return Content("<p style='color:red'> Invalid Link, Please Contact Administrator </p>");
                }
                else
                {
                    if (dlink.OpenStatus == false)
                    {
                        return Content("<p style='color:red'>Link Expired, Please Contact Administrator </p>");
                    }
                    else
                    {
                        // If Open Check Form Type, Action , send Action along with Form, then enable corresponding signature section and variable to submit identification

                        if (dlink.FormType == "NewHire")
                        {
                            ViewBag.Action = dlink.Action;
                            ViewBag.ReferenceId = dlink.DynamicLinkId;
                            ViewBag.IsCorporate = newhire.iscorporate;

                            dynamic obj = new { };

                            using (var client = new HttpClient())
                            {
                                //Passing service base url  
                                client.BaseAddress = new Uri(Baseurl);

                                client.DefaultRequestHeaders.Clear();
                                //Define request data format  
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeNewHireNotice?riderid=" + dlink.ReferenceId);
                                //Checking the response is successful or not which is sent using HttpClient  
                                if (Res.IsSuccessStatusCode)
                                {
                                    //Storing the response details recieved from web api   
                                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                                    //Deserializing the response recieved from web api and storing into the Employee list  
                                    obj = JsonConvert.DeserializeObject<PrintEmployeeNewHireNotice>(EmpResponse);

                                }

                                // o.Date=obj.
                                //returning the employee list to view  
                                return View("VerifyEmployeeNewHireNotice", obj);
                            }
                        }
                        else if (dlink.FormType == "LeaseRider")
                        {
                            ViewBag.Action = dlink.Action;
                            ViewBag.ReferenceId = dlink.DynamicLinkId;


                            dynamic obj = new { };

                            using (var client = new HttpClient())
                            {
                                //Passing service base url  
                                client.BaseAddress = new Uri(Baseurl);

                                client.DefaultRequestHeaders.Clear();
                                //Define request data format  
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid=" + dlink.ReferenceId);
                                //Checking the response is successful or not which is sent using HttpClient  
                                if (Res.IsSuccessStatusCode)
                                {
                                    //Storing the response details recieved from web api   
                                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                                    //Deserializing the response recieved from web api and storing into the Employee list  
                                    obj = JsonConvert.DeserializeObject<PrintEmployeeLeaseRider>(EmpResponse);

                                }

                                // o.Date=obj.
                                //returning the employee list to view  
                                return View("VerifyEmployeeLeaseRider", obj);
                            }
                        }
                        else if (dlink.FormType == "PayRoll")
                        {
                            ViewBag.Action = dlink.Action;
                            ViewBag.ReferenceId = dlink.DynamicLinkId;


                            dynamic obj = new { };

                            using (var client = new HttpClient())
                            {
                                //Passing service base url  
                                client.BaseAddress = new Uri(Baseurl);

                                client.DefaultRequestHeaders.Clear();
                                //Define request data format  
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                                HttpResponseMessage Res = await client.GetAsync("api/data/GetPayRollStatusChangeNotice?riderid=" + dlink.ReferenceId);
                                //Checking the response is successful or not which is sent using HttpClient  
                                if (Res.IsSuccessStatusCode)
                                {
                                    //Storing the response details recieved from web api   
                                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                                    //Deserializing the response recieved from web api and storing into the Employee list  
                                    obj = JsonConvert.DeserializeObject<PrintPayRollStatusChange>(EmpResponse);

                                }

                                // o.Date=obj.
                                //returning the employee list to view  
                                return View("VerifyPayRollStatusChange", obj);
                            }
                        }
                        else
                        {
                            return Content("Invalid Form Selection");
                        }
                    }
                }
            }



        }


        public ActionResult Open(string link)
        {
            ViewBag.link = link;
            return View();
        }


      public  dynamic SendClaimUpdatesLastWeek()
        {
            return WorkflowHelper.SendClaimUpdatesLastweek();
        }



        [AllowAnonymous]
        [ActionName("UpdateWorkflowEmployeeLeaseRiderAsync")]
        [HttpPost]
        [MyIgnore]
        public async Task<JsonResult> UpdateWorkflowEmployeeLeaseRiderAsync()
        {

            var action = Request.Params["action"].ToString();
            var refid = Request.Params["refid"].ToString();
            var signature = Request.Params["signature"].ToString();
            var date = DateTime.Now;
            var empname = Request.Params["empname"].ToString();
            // ip address .

            string VisitorsIPAddress = string.Empty;
            try
            {
                if (HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    VisitorsIPAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else if (HttpContext.Request.UserHostAddress.Length != 0)
                {
                    VisitorsIPAddress = HttpContext.Request.UserHostAddress;
                }
            }
            catch (Exception ex)
            {

                //Handle Exceptions  
            }
            // browser information 
            string browserDetails = string.Empty;
            System.Web.HttpBrowserCapabilitiesBase browser = HttpContext.Request.Browser;
            browserDetails =
            "Name = " + browser.Browser + "," +
            "Type = " + browser.Type + ","
            + "Version = " + browser.Version + ","
            + "Major Version = " + browser.MajorVersion + ","
            + "Minor Version = " + browser.MinorVersion + ","
            + "Platform = " + browser.Platform + ","
            + "Is Beta = " + browser.Beta + ","
            + "Is Crawler = " + browser.Crawler + ","
            + "Is AOL = " + browser.AOL + ","
            + "Is Win16 = " + browser.Win16 + ","
            + "Is Win32 = " + browser.Win32 + ","
            + "Supports Frames = " + browser.Frames + ","
            + "Supports Tables = " + browser.Tables + ","
            + "Supports Cookies = " + browser.Cookies + ","
            + "Supports VBScript = " + browser.VBScript + ","
            + "Supports JavaScript = " + "," +
            browser.EcmaScriptVersion.ToString() + ","
            + "Supports Java Applets = " + browser.JavaApplets + ","
            + "Supports ActiveX Controls = " + browser.ActiveXControls
            + ","
            + "Supports JavaScript Version = " +
            browser["JavaScriptVersion"];

            var _service = new EntityDataRepository();

            if (_service.CheckEmployeeLeaseRiderBeforeSubmit(refid))
            {
                return Json("Submitted");
            }
            else
            {

            var retu = _service.UpdateWorkflowEmployeeLeaseRider(action, refid, signature, date, browserDetails, VisitorsIPAddress);

            WorkflowHelper.InsertHrLog("LeaseRider", retu, "Employee Signature has been completed", "Employee Signature has been Submitted for Employee Lease Rider on" + DateTime.Now, empname);

            var Message = WorkflowHelper.SendHrWorkFlowEmail(retu, "LeaseRider", "Manager Email", "System");

            // write your email function here..
            MailMessage mail = new MailMessage();


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(Message.Body,
                    null, MediaTypeNames.Text.Html);

            SmtpClient smtp = EmailHelper.SetMailServerSettings();
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            foreach (var item in Message.EmailTo)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Match match = regex.Match(item);
                    if (match.Success)
                    {
                        mail.To.Add(item);
                    }
                }
            }

            mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Carroll Organization");


            //foreach (var item in Message.EmailCc)
            //{
            //    mail.CC.Add(new MailAddress(item));
            //}

            mail.AlternateViews.Add(av1);

            mail.IsBodyHtml = true;

            dynamic obj = new { };

            var client = new HttpClient();

            //Passing service base url  
            client.BaseAddress = new Uri(Baseurl);

            client.DefaultRequestHeaders.Clear();
            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
            //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
            HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid=" + retu);

            //Checking the response is successful or not which is sent using HttpClient  
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list  
                obj = JsonConvert.DeserializeObject<PrintEmployeeLeaseRider>(EmpResponse);
            }
            HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + retu + "&FormType=" + "LeaseRider");
            //Checking the response is successful or not which is sent using HttpClient  
            if (Res1.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list  
                obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

            }

            var actionPDF = new Rotativa.ViewAsPdf("../Hr/PrintEmployeeLeaseRider", obj)//some route values)
            {
                //FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                CustomSwitches = "--disable-smart-shrinking",
                PageMargins = { Left = 1, Right = 1 }
            };


            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

            MemoryStream file = new MemoryStream(applicationPDFData);
            file.Seek(0, SeekOrigin.Begin);

            Attachment data = new Attachment(file, (string)obj.EmployeeName + "_EmployeeLeaseRider" + ".pdf", "application/pdf");

            System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.DateTime.Now;
            disposition.ModificationDate = System.DateTime.Now;
            disposition.DispositionType = DispositionTypeNames.Attachment;

            mail.Attachments.Add(data);

            mail.Subject = Message.Subject;
            mail.Body = Message.Body;
            //   mail.To.Clear();
            // remove this line before going production
          

            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.Priority = MailPriority.High;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {

            }

                return Json(retu);

            }



           
        }


        [AllowAnonymous]
        [ActionName("GetDynamicLinkStatus")]
        [HttpGet]
        public async Task<dynamic>  GetDynamicLinkStatus(string refid)
        {

            var client = new HttpClient();

            //Passing service base url  
            client.BaseAddress = new Uri(Baseurl);

            client.DefaultRequestHeaders.Clear();
            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
            //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
            HttpResponseMessage Res = await client.GetAsync("api/data/GetDynamicLinkStatus?refid=" + refid);
             bool fi = false;

            //Checking the response is successful or not which is sent using HttpClient  
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                
                //Deserializing the response recieved from web api and storing into the Employee list  
                fi = JsonConvert.DeserializeObject<bool>(EmpResponse);
            }

            return fi;
        }
                       
        [AllowAnonymous]
        [ActionName("UpdateWorkflowPayRollStatusChangeNoticeAsync")]
        [HttpPost]
        [MyIgnore]       
        public async Task<JsonResult> UpdateWorkflowPayRollStatusChangeNoticeAsync()
        {

            var action = Request.Params["action"].ToString();
            var refid = Request.Params["refid"].ToString();
            var signature = Request.Params["signature"].ToString();
        //    var date = Convert.ToDateTime(Request.Params["date"].ToString());
            var date =DateTime.Now;
            var empname = Request.Params["empname"].ToString();
            // ip address 

            string VisitorsIPAddress = string.Empty;
            try
            {
                if (HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    VisitorsIPAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else if (HttpContext.Request.UserHostAddress.Length != 0)
                {
                    VisitorsIPAddress = HttpContext.Request.UserHostAddress;
                }
            }
            catch (Exception ex)
            {

                //Handle Exceptions  
            }
            // browser information 
            string browserDetails = string.Empty;
            System.Web.HttpBrowserCapabilitiesBase browser = HttpContext.Request.Browser;
            browserDetails =
            "Name = " + browser.Browser + "," +
            "Type = " + browser.Type + ","
            + "Version = " + browser.Version + ","
            + "Major Version = " + browser.MajorVersion + ","
            + "Minor Version = " + browser.MinorVersion + ","
            + "Platform = " + browser.Platform + ","
            + "Is Beta = " + browser.Beta + ","
            + "Is Crawler = " + browser.Crawler + ","
            + "Is AOL = " + browser.AOL + ","
            + "Is Win16 = " + browser.Win16 + ","
            + "Is Win32 = " + browser.Win32 + ","
            + "Supports Frames = " + browser.Frames + ","
            + "Supports Tables = " + browser.Tables + ","
            + "Supports Cookies = " + browser.Cookies + ","
            + "Supports VBScript = " + browser.VBScript + ","
            + "Supports JavaScript = " + "," +
            browser.EcmaScriptVersion.ToString() + ","
            + "Supports Java Applets = " + browser.JavaApplets + ","
            + "Supports ActiveX Controls = " + browser.ActiveXControls
            + ","
            + "Supports JavaScript Version = " +
            browser["JavaScriptVersion"];




            var _service = new EntityDataRepository();

            if(_service.checkseperationbeforesubmission(refid))
            {
                return Json("Submitted");
            }
            else
            {

            var retu = _service.UpdateWorkflowPayRollStatusChangeNotice(action, refid, signature, date, browserDetails, VisitorsIPAddress);

            if (Session["user"] != null)
            {
                var user = (SiteUser)Session["user"];

                WorkflowHelper.InsertHrLog("PayRoll", retu, "Employee Signature has been completed", "Employee Signature has been Submitted for Payroll Status Change on" + DateTime.Now, empname);

            }
            else
                WorkflowHelper.InsertHrLog("PayRoll", retu, "Employee Signature has been completed", "Employee Signature has been Submitted for Payroll Status Change on" + DateTime.Now, empname);


            var Message = WorkflowHelper.SendHrWorkFlowEmail(retu, "PayRoll", "Manager Email", "System");

            // write your email function here..
            MailMessage mail = new MailMessage();


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(Message.Body,
                    null, MediaTypeNames.Text.Html);

            SmtpClient smtp = EmailHelper.SetMailServerSettings();
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            foreach (var item in Message.EmailTo)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Match match = regex.Match(item);
                    if (match.Success)
                    {
                        mail.To.Add(item);
                    }
                }
            }

            mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Carroll Organization");


            //foreach (var item in Message.EmailCc)
            //{
            //    mail.CC.Add(new MailAddress(item));
            //}

            mail.AlternateViews.Add(av1);

            mail.IsBodyHtml = true;

            dynamic obj = new { };

            var client = new HttpClient();

            //Passing service base url  
            client.BaseAddress = new Uri(Baseurl);

            client.DefaultRequestHeaders.Clear();
            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
            //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
            HttpResponseMessage Res = await client.GetAsync("api/data/GetPayRollStatusChangeNotice?riderid=" + retu);

            //Checking the response is successful or not which is sent using HttpClient  
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var EmpResponse = Res.Content.ReadAsStringAsync().Result;


                //Deserializing the response recieved from web api and storing into the Employee list  
                obj = JsonConvert.DeserializeObject<PrintPayRollStatusChange>(EmpResponse);


            }
            HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + retu + "&FormType=" + "PayRoll");
            //Checking the response is successful or not which is sent using HttpClient  
            if (Res1.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list  
                obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

            }


            var actionPDF = new Rotativa.ViewAsPdf("../Hr/PrintPayRollStatusChange", obj)//some route values)
            {
                //FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                CustomSwitches = "--disable-smart-shrinking",
                PageMargins = { Left = 1, Right = 1 }
            };


            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

            MemoryStream file = new MemoryStream(applicationPDFData);
            file.Seek(0, SeekOrigin.Begin);

            Attachment data = new Attachment(file, (string)obj.EmployeeName + "_PayRollStatusChange" + ".pdf", "application/pdf");

            System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.DateTime.Now;
            disposition.ModificationDate = System.DateTime.Now;
            disposition.DispositionType = DispositionTypeNames.Attachment;

            mail.Attachments.Add(data);

            mail.Subject = Message.Subject;
            mail.Body = Message.Body;
            //   mail.To.Clear();
            // remove this line before going production
        

            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.Priority = MailPriority.High;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {

            }



                // send service desk email 

                var Message2 = WorkflowHelper.SendHrWorkFlowEmail(retu, "PayRoll", "Service Email", "System");

                // write your email function here..
                MailMessage mailservicedesk = new MailMessage();


                AlternateView av2 = AlternateView.CreateAlternateViewFromString(Message2.Body,
                        null, MediaTypeNames.Text.Html);

                SmtpClient smtp1 = EmailHelper.SetMailServerSettings();
                Regex regex1 = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                foreach (var item in Message2.EmailTo)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        Match match = regex.Match(item);
                        if (match.Success)
                        {
                            mailservicedesk.To.Add(item);
                        }
                    }
                }

                mailservicedesk.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Carroll Organization");


                //foreach (var item in Message.EmailCc)
                //{
                //    mail.CC.Add(new MailAddress(item));
                //}

                mailservicedesk.AlternateViews.Add(av2);

                mailservicedesk.IsBodyHtml = true;





                mailservicedesk.Subject = Message2.Subject;
                mailservicedesk.Body = Message2.Body;
                //   mail.To.Clear();

                mailservicedesk.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mailservicedesk.Priority = MailPriority.High;
                try
                {
                    smtp.Send(mailservicedesk);
                }
                catch (Exception ex)
                {

                }

                return Json(retu);

            }


           
        }

        [AllowAnonymous]
        [ActionName("UpdateWorkflowEmployeeNewHireNotice")]
        [HttpPost]
        [MyIgnore]
        public async Task<JsonResult> UpdateWorkflowEmployeeNewHireNoticeAsync(string action, string refid, string signature, string iscorporate,string empname)
        {

            //var Action = Request.Params["action"].ToString();
            //var Refid = Request.Params["refid"].ToString();
            //var Sign = Request.Params["signature"].ToString();
            //var iscorporate = Request.Params["iscorporate"].ToString();

            DateTime? edate = DateTime.Now;
            //if (!string.IsNullOrEmpty(Request.Params["date"]))
            //{
            //    edate = Convert.ToDateTime(Request.Params["date"].ToString());
            //}


            // ip address 

            string VisitorsIPAddress = string.Empty;
            try
            {
                if (HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    VisitorsIPAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else if (HttpContext.Request.UserHostAddress.Length != 0)
                {
                    VisitorsIPAddress = HttpContext.Request.UserHostAddress;
                }
            }
            catch (Exception ex)
            {

                //Handle Exceptions  
            }
            // browser information 
            string browserDetails = string.Empty;
            System.Web.HttpBrowserCapabilitiesBase browser = HttpContext.Request.Browser;
            browserDetails =
            "Name = " + browser.Browser + "," +
            "Type = " + browser.Type + ","
            + "Version = " + browser.Version + ","
            + "Major Version = " + browser.MajorVersion + ","
            + "Minor Version = " + browser.MinorVersion + ","
            + "Platform = " + browser.Platform + ","
            + "Is Beta = " + browser.Beta + ","
            + "Is Crawler = " + browser.Crawler + ","
            + "Is AOL = " + browser.AOL + ","
            + "Is Win16 = " + browser.Win16 + ","
            + "Is Win32 = " + browser.Win32 + ","
            + "Supports Frames = " + browser.Frames + ","
            + "Supports Tables = " + browser.Tables + ","
            + "Supports Cookies = " + browser.Cookies + ","
            + "Supports VBScript = " + browser.VBScript + ","
            + "Supports JavaScript = " + "," +
            browser.EcmaScriptVersion.ToString() + ","
            + "Supports Java Applets = " + browser.JavaApplets + ","
            + "Supports ActiveX Controls = " + browser.ActiveXControls
            + ","
            + "Supports JavaScript Version = " +
            browser["JavaScriptVersion"];



            var _service = new EntityDataRepository();
            if(_service.checknewhirebeforesubmission(refid,action))
            {
                return Json("Submitted");
            }
            else
            {


            var retu = _service.UpdateWorkflowEmployeeNewHireNotice(action, refid, signature, edate, browserDetails, VisitorsIPAddress);


            if (action == "Employee Email" && iscorporate.ToLower() == "false")
            {
              //  var empemail = Request.Params["empemail"].ToString();

                WorkflowHelper.InsertHrLog("NewHire", retu, "Employee Signature has been completed", "Employee Signature has been Submitted for New Hire Notice on" + DateTime.Now, empname);

                WorkflowHelper.SendHrWorkFlowEmail(retu, "NewHire", "Regional Email", "System");


            }
            else if (action == "Employee Email" && iscorporate.ToLower() == "true")
            {
                //var mgr = WorkflowHelper.GetMgrNamefromnewhire(retu);
                //WorkflowHelper.InsertHrLog("NewHire", retu, "Regional Signature has been completed ", "Employee Signature Submitted for New Hire Notice on" + DateTime.Now, mgr);

                // WorkflowHelper.UpdatePmBrowserInfo(retu, "NewHire", "Regional Email", browserDetails, VisitorsIPAddress);
                WorkflowHelper.InsertHrLog("NewHire", retu, "Employee Signature has been completed", "Employee Signature has been Submitted for New Hire Notice on" + DateTime.Now, empname);

                var Message = WorkflowHelper.SendHrWorkFlowEmail(retu, "NewHire", "Manager Email", "System");

                // write your email function here..
                MailMessage mail = new MailMessage();


                AlternateView av1 = AlternateView.CreateAlternateViewFromString(Message.Body,
                        null, MediaTypeNames.Text.Html);

                SmtpClient smtp = EmailHelper.SetMailServerSettings();
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                foreach (var item in Message.EmailTo)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        Match match = regex.Match(item);
                        if (match.Success)
                        {
                            mail.To.Add(item);
                        }
                    }
                }

                mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Carroll Organization");


                //foreach (var item in Message.EmailCc)
                //{
                //    mail.CC.Add(new MailAddress(item));
                //}

                mail.AlternateViews.Add(av1);

                mail.IsBodyHtml = true;

                dynamic obj = new { };

                var client = new HttpClient();

                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeNewHireNotice?riderid=" + retu);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintEmployeeNewHireNotice>(EmpResponse);


                }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + retu + "&FormType=" + "NewHire");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                var actionPDF = new Rotativa.ViewAsPdf("../Hr/PrintEmployeeNewHireNotice", obj)//some route values)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    CustomSwitches = "--disable-smart-shrinking",
                    PageMargins = { Left = 1, Right = 1 }
                };


                byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

                MemoryStream file = new MemoryStream(applicationPDFData);
                file.Seek(0, SeekOrigin.Begin);

                Attachment data = new Attachment(file, (string)obj.EmployeeName + "_EmployeeNewHireNotice" + ".pdf", "application/pdf");

                System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.DateTime.Now;
                disposition.ModificationDate = System.DateTime.Now;
                disposition.DispositionType = DispositionTypeNames.Attachment;

                mail.Attachments.Add(data);

                mail.Subject = Message.Subject;
                mail.Body = Message.Body;
                //   mail.To.Clear();
                // remove this line before going production
                     

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mail.Priority = MailPriority.High;
                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {

                }


                    // send service desk email 

                    var Message2 = WorkflowHelper.SendHrWorkFlowEmail(retu, "NewHire", "Service Email", "System");

                    // write your email function here..
                    MailMessage mailservicedesk = new MailMessage();


                    AlternateView av2 = AlternateView.CreateAlternateViewFromString(Message2.Body,
                            null, MediaTypeNames.Text.Html);

                    SmtpClient smtp1 = EmailHelper.SetMailServerSettings();
                    Regex regex1 = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    foreach (var item in Message2.EmailTo)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            Match match = regex.Match(item);
                            if (match.Success)
                            {
                                mailservicedesk.To.Add(item);
                            }
                        }
                    }

                    mailservicedesk.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Carroll Organization");


                    //foreach (var item in Message.EmailCc)
                    //{
                    //    mail.CC.Add(new MailAddress(item));
                    //}

                    mailservicedesk.AlternateViews.Add(av2);

                    mailservicedesk.IsBodyHtml = true;





                    mailservicedesk.Subject = Message2.Subject;
                    mailservicedesk.Body = Message2.Body;
                    //   mail.To.Clear();

                    mailservicedesk.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    mailservicedesk.Priority = MailPriority.High;
                    try
                    {
                        smtp.Send(mailservicedesk);
                    }
                    catch (Exception ex)
                    {

                    }


                }

                else
            {
                var mgr = WorkflowHelper.GetMgrNamefromnewhire(retu);
                WorkflowHelper.InsertHrLog("NewHire", retu, "Regional Signature has been completed ", "Employee Signature Submitted for New Hire Notice on" + DateTime.Now, mgr);

                // WorkflowHelper.UpdatePmBrowserInfo(retu, "NewHire", "Regional Email", browserDetails, VisitorsIPAddress);

                var Message = WorkflowHelper.SendHrWorkFlowEmail(retu, "NewHire", "Manager Email", "System");

                // write your email function here..
                MailMessage mail = new MailMessage();


                AlternateView av1 = AlternateView.CreateAlternateViewFromString(Message.Body,
                        null, MediaTypeNames.Text.Html);

                SmtpClient smtp = EmailHelper.SetMailServerSettings();
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                foreach (var item in Message.EmailTo)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        Match match = regex.Match(item);
                        if (match.Success)
                        {
                            mail.To.Add(item);
                        }
                    }
                }

                mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Carroll Organization");


                //foreach (var item in Message.EmailCc)
                //{
                //    mail.CC.Add(new MailAddress(item));
                //}

                mail.AlternateViews.Add(av1);

                mail.IsBodyHtml = true;

                dynamic obj = new { };

                var client = new HttpClient();

                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeNewHireNotice?riderid=" + retu);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintEmployeeNewHireNotice>(EmpResponse);


                }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + retu + "&FormType=" + "NewHire");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                var actionPDF = new Rotativa.ViewAsPdf("../Hr/PrintEmployeeNewHireNotice", obj)//some route values)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    CustomSwitches = "--disable-smart-shrinking",
                    PageMargins = { Left = 1, Right = 1 }
                };


                byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

                MemoryStream file = new MemoryStream(applicationPDFData);
                file.Seek(0, SeekOrigin.Begin);

                Attachment data = new Attachment(file, (string)obj.EmployeeName + "_EmployeeNewHireNotice" + ".pdf", "application/pdf");

                System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.DateTime.Now;
                disposition.ModificationDate = System.DateTime.Now;
                disposition.DispositionType = DispositionTypeNames.Attachment;

                mail.Attachments.Add(data);

                mail.Subject = Message.Subject;
                mail.Body = Message.Body;
                //   mail.To.Clear();
            
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mail.Priority = MailPriority.High;
                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {

                }

                    // send service desk email 

                     var Message2 = WorkflowHelper.SendHrWorkFlowEmail(retu, "NewHire", "Service Email", "System");

                    // write your email function here..
                    MailMessage mailservicedesk = new MailMessage();


                    AlternateView av2 = AlternateView.CreateAlternateViewFromString(Message2.Body,
                            null, MediaTypeNames.Text.Html);

                    SmtpClient smtp1 = EmailHelper.SetMailServerSettings();
                    Regex regex1 = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    foreach (var item in Message2.EmailTo)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            Match match = regex.Match(item);
                            if (match.Success)
                            {
                                mailservicedesk.To.Add(item);
                            }
                        }
                    }

                    mailservicedesk.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Carroll Organization");


                    //foreach (var item in Message.EmailCc)
                    //{
                    //    mail.CC.Add(new MailAddress(item));
                    //}

                    mailservicedesk.AlternateViews.Add(av2);

                    mailservicedesk.IsBodyHtml = true;

                   

                  

                    mailservicedesk.Subject = Message2.Subject;
                    mailservicedesk.Body = Message2.Body;
                    //   mail.To.Clear();

                    mailservicedesk.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    mailservicedesk.Priority = MailPriority.High;
                    try
                    {
                        smtp.Send(mailservicedesk);
                    }
                    catch (Exception ex)
                    {

                    }


                }
                return Json(retu);
            }

           
        }
        
    }
}