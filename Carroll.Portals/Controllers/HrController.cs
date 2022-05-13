using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Carroll.Portals.Helpers;
using Carroll.Portals.Models;
using Carroll.Data.Entities.Helpers;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Services.Helpers;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Rotativa;
using System.IO;
using Rotativa.Options;
using ClosedXML.Excel;
using System.Data;
using ClosedXML.Extensions;
using System.Reflection;
using Carroll.Data.Entities;

namespace Carroll.Portals.Controllers
{
    [CustomAuthorize]
    [BaseModel]
   // [AdminFilter]
    [ExceptionFilter]
    public class HrController : Controller
    {

        string Baseurl = "";

        public HrController()
        {
            Baseurl = ConfigurationManager.AppSettings["ServiceURL"];
            //remove before live
         //  Baseurl = "http://localhost:1002/";
        }

        public ActionResult Index()
        {
            return View(new BaseViewModel());
        }

        // GET: Hr
        public async Task<ActionResult> PrintEmployeeLeaseRider(string id)
        {
            PrintEmployeeLeaseRider obj = new PrintEmployeeLeaseRider();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintEmployeeLeaseRider>(EmpResponse);
                }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "LeaseRider");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                if (Session["user"] != null)
                {
                    var user = (SiteUser)Session["user"];
                    WorkflowHelper.InsertHrLog("LeaseRider", id, "Print has been Requested ", "Print has been requested for Employee Lease Rider on" + DateTime.Now,  FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));

                   
                }


                // o.Date=obj.
                //returning the employee list to view  
                return View(obj);
            }

        }

        public async Task<ActionResult> PdfEmployeeLeaseRider(string id)
        {

            PrintEmployeeLeaseRider obj = new PrintEmployeeLeaseRider();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintEmployeeLeaseRider>(EmpResponse);

                }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "LeaseRider");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }
                if (Session["user"] != null)
                {
                    var user = (SiteUser)Session["user"];
                    WorkflowHelper.InsertHrLog("LeaseRider", id, "PDF has been requested", "PDF has been requestedfor Employee Lease Rider on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));

                }
               
                // o.Date=obj.
                //returning the employee list to view  
                return new ViewAsPdf("PrintEmployeeLeaseRider", obj) { PageSize= Size.A4, CustomSwitches= "--disable-smart-shrinking", FileName = "EmployeeLeaseRider-" +obj.SequenceNumber +"-"+obj.EmployeeName+ ".pdf" };

            }

        }

        public ActionResult EmployeeLeaseRider()
        {
            return View(new BaseViewModel());
        }

        public ActionResult EmployeeNewHireNotice()
        {
            return View(new BaseViewModel());
        }

        [AllowAnonymous]
        public async Task<ActionResult> PrintEmployeeNewHireNotice(string id)
        {


            PrintEmployeeNewHireNotice obj = new PrintEmployeeNewHireNotice();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeNewHireNotice?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintEmployeeNewHireNotice>(EmpResponse);
                }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "NewHire");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }
                if (Session["user"] != null)
                {
                    var user = (SiteUser)Session["user"];

                    WorkflowHelper.InsertHrLog("NewHire", id, "Print has been requested ", "Print has been requested for New Hire Notice on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));


                }

                // o.Date=obj.
                //returning the employee list to view  
                return View("PrintEmployeeNewHireNotice", obj);
            }

        }

        public async Task<ActionResult> PdfEmployeeNewHireNotice(string id)
        {


            PrintEmployeeNewHireNotice obj = new PrintEmployeeNewHireNotice();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeNewHireNotice?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintEmployeeNewHireNotice>(EmpResponse);

                }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "NewHire");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }
                // o.Date=obj.
                //returning the employee list to view  
                //      return View(obj);

                if (Session["user"] != null)
                {
                    var user = (SiteUser)Session["user"];
                    WorkflowHelper.InsertHrLog("NewHire", id, "PDF has been requested", "PDF has been requestedfor New Hire Notice on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));

                }
               

                //returning the employee list to view  
                return new ViewAsPdf("PrintEmployeeNewHireNotice", obj) { PageSize = Size.A4, CustomSwitches = "--disable-smart-shrinking", FileName = "EmployeeNewHireNotice-" +obj.SequenceNumber + "-" + obj.EmployeeName  + ".pdf" };

            }
        }

        public async Task<ActionResult> ExportActivity(string Id,string FormType,string rid)
        {


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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + Id+"&FormType="+FormType);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);
                    
                }

                // o.Date=obj.
                //returning the employee list to view  
                //      return View(obj);

                if (Session["user"] != null)
                {
                    var user = (SiteUser)Session["user"];
                    WorkflowHelper.InsertHrLog(FormType, Id, "Activity Export has been requested", "Activity has been requested on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " " + FirstCharToUpper(user.LastName));

                }

                ViewBag.Id = rid;
                //returning the employee list to view  
                return new ViewAsPdf("PrintActivityExport", obj) { PageSize = Size.A4, CustomSwitches = "--disable-smart-shrinking", FileName = "ActivtyExport-" + rid + ".pdf" };

            }
        }

        [AllowAnonymous]
        [ActionName("UpdateWorkflowEmployeeLeaseRiderAsync")]
        [HttpPost]
        [MyIgnore]
        public async Task<dynamic> UpdateWorkflowEmployeeLeaseRiderAsync()
        {

            var action = Request.Params["action"].ToString();
            var refid = Request.Params["refid"].ToString();
            var signature = Request.Params["signature"].ToString();
            var date = Convert.ToDateTime(Request.Params["date"].ToString());
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
            var retu = _service.UpdateWorkflowEmployeeLeaseRider(action, refid, signature, date, browserDetails, VisitorsIPAddress);

             WorkflowHelper.InsertHrLog("LeaseRider", retu, "Employee Signature has been completed", "Employee Signature has been Submitted for Employee Lease Rider on" + DateTime.Now, empname);

            var Message = WorkflowHelper.SendHrWorkFlowEmail(retu, "LeaseRider", "Manager Email","System");

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

            PrintEmployeeLeaseRider obj = new PrintEmployeeLeaseRider();

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
            var actionPDF = new Rotativa.ViewAsPdf("PrintEmployeeLeaseRider", obj)//some route values)
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





            return retu;
        }


        [AllowAnonymous]
        [ActionName("UpdateWorkflowPayRollStatusChangeNoticeAsync")]
        [HttpPost]
        [MyIgnore]
        public async Task<dynamic> UpdateWorkflowPayRollStatusChangeNoticeAsync()
        {

            var action = Request.Params["action"].ToString();
            var refid = Request.Params["refid"].ToString();
            var signature = Request.Params["signature"].ToString();
            var date = Convert.ToDateTime(Request.Params["date"].ToString());
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
            var retu = _service.UpdateWorkflowPayRollStatusChangeNotice(action, refid, signature, date, browserDetails, VisitorsIPAddress);

            if (Session["user"] != null)
            {
                var user = (SiteUser)Session["user"];

            WorkflowHelper.InsertHrLog("PayRoll", retu, "Employee Signature has been completed", "Employee Signature has been Submitted for Payroll Status Change on" + DateTime.Now, empname);
                
            }

            var Message = WorkflowHelper.SendHrWorkFlowEmail(retu, "PayRoll", "Manager Email","System");

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

            PrintPayRollStatusChange obj = new PrintPayRollStatusChange() ;

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


            var actionPDF = new Rotativa.ViewAsPdf("PrintPayRollStatusChange", obj)//some route values)
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
         

            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.Priority = MailPriority.High;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {

            }





            return retu;
        }

        [AllowAnonymous]
        [ActionName("UpdateWorkflowEmployeeNewHireNotice")]
        [HttpPost]
        [MyIgnore]
        public async Task<dynamic> UpdateWorkflowEmployeeNewHireNoticeAsync(string action,string refid,string signature,string iscorporate)
        {

            //var Action = Request.Params["action"].ToString();
            //var Refid = Request.Params["refid"].ToString();
            //var Sign = Request.Params["signature"].ToString();
            //var iscorporate = Request.Params["iscorporate"].ToString();
           
            DateTime? edate = null;
            if (!string.IsNullOrEmpty(Request.Params["date"]))
            {
                edate = Convert.ToDateTime(Request.Params["date"].ToString());
            }


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

            var retu = _service.UpdateWorkflowEmployeeNewHireNotice(action, refid, signature, edate,browserDetails,VisitorsIPAddress);
          

            if (action == "Employee Email" && iscorporate.ToLower() == "false")
            {
                var empemail = Request.Params["empemail"].ToString();

                WorkflowHelper.InsertHrLog("NewHire", retu, "Employee Signature has been completed", "Employee Signature has been Submitted for New Hire Notice on" + DateTime.Now, empemail);

                WorkflowHelper.SendHrWorkFlowEmail(retu, "NewHire", "Regional Email", "System");

              
            }
            else
            {
                WorkflowHelper.InsertHrLog("NewHire", retu, "Regional Signature has been completed ", "Regional Signature Submitted for New Hire Notice on" + DateTime.Now, "System");

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

                PrintEmployeeNewHireNotice obj = new PrintEmployeeNewHireNotice();

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

                var actionPDF = new Rotativa.ViewAsPdf("PrintEmployeeNewHireNotice", obj)//some route values)
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

                Attachment data = new Attachment(file, (string)obj.EmployeeName+ "_EmployeeNewHireNotice" + ".pdf", "application/pdf");
               
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
                catch(Exception ex)
                {

                }
                    


                }

                return retu;
        }



        [MyIgnore]
        [OverrideAuthorization]
        [OverrideActionFilters]
        [AllowAnonymous]
        [ActionName("SendHrFormCompletionNotification")]
        [HttpPost]
       
        public async Task<dynamic> SendHrFormCompletionNotification(string form, string refid)
        {
          

            var Message = WorkflowHelper.ReSendHrWorkFlowEmail(refid,form,"","System");

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

                if(form == "EmployeeLeaseRider")
                {
                                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid=" + refid);
                                //Checking the response is successful or not which is sent using HttpClient  
                                if (Res.IsSuccessStatusCode)
                                {
                                    //Storing the response details recieved from web api   
                                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                                    //Deserializing the response recieved from web api and storing into the Employee list  
                                    obj = JsonConvert.DeserializeObject<PrintEmployeeLeaseRider>(EmpResponse);

                                }


                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + refid + "&FormType=" + "LeaseRider");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                var actionPDF = new Rotativa.ViewAsPdf("PrintEmployeeLeaseRider", obj)//some route values)
                                {
                                    //FileName = "TestView.pdf",
                                    PageSize = Size.A4,
                                    CustomSwitches = "--disable-smart-shrinking",
                                    PageOrientation = Rotativa.Options.Orientation.Portrait,
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



                }
                else if(form == "NoticeOfEmployeeSeparation")
                {
                    HttpResponseMessage Res = await client.GetAsync("api/data/GetNoticeOfEmployeeSeperation?riderid=" + refid);
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        obj = JsonConvert.DeserializeObject<PrintNoticeOfEmployeeSeparation>(EmpResponse);

                    }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + refid + "&FormType=" + "NoticeOfEmployeeSeparation");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                var actionPDF = new Rotativa.ViewAsPdf("PrintNoticeOfEmployeeSeparation", obj)//some route values)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Size.A4,
                        CustomSwitches = "--disable-smart-shrinking",
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 }
                    };


                    byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

                    MemoryStream file = new MemoryStream(applicationPDFData);
                    file.Seek(0, SeekOrigin.Begin);

                    Attachment data = new Attachment(file, (string)obj.EmployeeName + "_NoticeOfEmployeeSeparation" + ".pdf", "application/pdf");

                    System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.DateTime.Now;
                    disposition.ModificationDate = System.DateTime.Now;
                disposition.DispositionType = DispositionTypeNames.Attachment;

                    mail.Attachments.Add(data);
                }
                else if(form == "RequisitionRequest")
                {
                    HttpResponseMessage Res = await client.GetAsync("api/data/GetRequisitionRequest?riderid=" + refid);
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        obj = JsonConvert.DeserializeObject<PrintRequisitionRequest>(EmpResponse);

                    }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + refid + "&FormType=" + "RequisitionRequest");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                var actionPDF = new Rotativa.ViewAsPdf("PrintRequisitionRequest", obj)//some route values)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Size.A4,
                        CustomSwitches = "--disable-smart-shrinking",
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 }
                    };


                    byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

                    MemoryStream file = new MemoryStream(applicationPDFData);
                    file.Seek(0, SeekOrigin.Begin);

                    Attachment data = new Attachment(file, (string)obj.PropertyName + "_RequisitionRequest" + ".pdf", "application/pdf");

                    System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.DateTime.Now;
                    disposition.ModificationDate = System.DateTime.Now;
                    disposition.DispositionType = DispositionTypeNames.Attachment;

                    mail.Attachments.Add(data);

                }



                mail.Subject = Message.Subject;
                mail.Body = Message.Body;
               // mail.To.Clear();
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
                return true;
           
        }

        
        public ActionResult PayRollStatusChange()
        {
            return View(new BaseViewModel());
        }

        public async Task<ActionResult> PrintPayRollStatusChange(string id)
        {


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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetPayRollStatusChangeNotice?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient
                
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintPayRollStatusChange>(EmpResponse);
                }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "PayRoll");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }
                SiteUser user = new SiteUser();
                if (Session["user"] != null)
                {
                    user = (SiteUser)Session["user"];
                }

                ViewBag.isprint = true;


                WorkflowHelper.InsertHrLog("PayRoll", id, "Print has been requested ", "Print has been requested for Payroll Status Change on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));


                // o.Date=obj.
                //returning the employee list to view  
                return View("PrintPayRollStatusChange", obj);
            }

        }

        public async Task<ActionResult> PdfPayRollStatusChange(string id)
        {
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetPayRollStatusChangeNotice?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintPayRollStatusChange>(EmpResponse);

                }
                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "PayRoll");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }
                // o.Date=obj.
                //returning the employee list to view  
                //      return View(obj);

                SiteUser user = new SiteUser();
                if (Session["user"] != null)
                {
                    user = (SiteUser)Session["user"];
                }

                WorkflowHelper.InsertHrLog("PayRoll", id, "PDF has been requested", "PDF has been requestedfor Payroll Status Change on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));

                ViewBag.isprint = false;
                //returning the employee list to view  
                return new ViewAsPdf("PdfPayRollStatusChange", obj) { PageSize = Size.A4, CustomSwitches = "--disable-smart-shrinking", FileName = "PayrollStatusChange-" + obj.SequenceNumber + "-" + obj.EmployeeName + ".pdf" };

            }

        }


        #region NotificeOfEmployeeSeparation 

        public ActionResult NoticeOfEmployeeSeparation()
        {
            return View(new BaseViewModel());
        }

        public async Task<ActionResult> PrintNoticeOfEmployeeSeparation(string id)
        {


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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetNoticeOfEmployeeSeperation?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintNoticeOfEmployeeSeparation>(EmpResponse);

                }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "NoticeOfEmployeeSeparation");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                if (Session["user"] != null)
                {
                    var user = (SiteUser)Session["user"];

                    WorkflowHelper.InsertHrLog("NoticeOfEmployeeSeparation", id, "Print has been requested ", "Print has been requested for Notice Of Employee Separation on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));

                }



                // o.Date=obj.
                //returning the employee list to view  
                return View("PrintNoticeOfEmployeeSeparation", obj);
            }

        }

        public async Task<ActionResult> PdfNoticeOfEmployeeSeparation(string id)
        {

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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetNoticeOfEmployeeSeperation?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintNoticeOfEmployeeSeparation>(EmpResponse);

                }

                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "NoticeOfEmployeeSeparation");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                // o.Date=obj.
                //returning the employee list to view  
                //      return View(obj);
                if (Session["user"] != null)
                {
                    var user = (SiteUser)Session["user"];
                    WorkflowHelper.InsertHrLog("NoticeOfEmployeeSeparation", id, "PDF has been requested", "PDF has been requestedfor Notice Of Employee Separation on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));

                }


           

                //returning the employee list to view  
                return new ViewAsPdf("PrintNoticeOfEmployeeSeparation", obj) { PageSize = Size.A4, CustomSwitches = "--disable-smart-shrinking", FileName = "NoticeOfEmployeeSeparation-" + obj.SequenceNumber + "-" + obj.EmployeeName + ".pdf" };

            }

        }

        #endregion


        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {
                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        [HttpGet]
        public async Task<ActionResult> ExportsContacts()
        {
            using (var wb = new XLWorkbook())
            {
                // IEnumerable<proc_order_excelproducts2_Result> = db.proc_order_excelproducts2(id);

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
                    HttpResponseMessage Res = await client.GetAsync("api/data/ExportsContacts");
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list  
                        obj = JsonConvert.DeserializeObject<List<Carroll.Portals.Models.proc_getcontactsforexcel_Result>>(EmpResponse);
                    }

                    DataTable dt = LINQResultToDataTable(obj);

                    // Add ClosedXML.Extensions in your using declarations

                    var ws=wb.Worksheets.Add(dt, "Contacts");
                    ws.Columns("A").Hide();

                    return wb.Deliver("Contacts List -" + DateTime.Now.ToShortDateString() + ".xlsx");

                    // or specify the content type:
                    //  return wb.Deliver("generatedFile.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }
            }

        }


        [HttpGet]
        public async Task<ActionResult> ExportEquityPartners()
        {
            using (var wb = new XLWorkbook())
            {
                // IEnumerable<proc_order_excelproducts2_Result> = db.proc_order_excelproducts2(id);

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
                    HttpResponseMessage Res = await client.GetAsync("api/data/ExportEquityPartners");
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list  
                        obj = JsonConvert.DeserializeObject<List<Carroll.Portals.Models.proc_getequitypartnersforexcel_Result>>(EmpResponse);
                    }

                    DataTable dt = LINQResultToDataTable(obj);

                    // Add ClosedXML.Extensions in your using declarations

                   var ws= wb.Worksheets.Add(dt, "EquityPartners");
                    ws.Columns("A").Hide();

                    return wb.Deliver("EquityPartners List -" + DateTime.Now.ToShortDateString() + ".xlsx");

                    // or specify the content type:
                    //  return wb.Deliver("generatedFile.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }
            }

        }

        [HttpGet]
        public async Task<ActionResult> ExportProperties()
        {
            using (var wb = new XLWorkbook())
            {
                // IEnumerable<proc_order_excelproducts2_Result> = db.proc_order_excelproducts2(id);

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
                    HttpResponseMessage Res = await client.GetAsync("api/data/ExportProperties");
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list  
                        obj = JsonConvert.DeserializeObject<List<Carroll.Portals.Models.proc_getpropertiesforexcel_Result>>(EmpResponse);
                    }

                    DataTable dt = LINQResultToDataTable(obj);

                    // Add ClosedXML.Extensions in your using declarations

                   var ws= wb.Worksheets.Add(dt, "Properties");
                    ws.Columns("A").Hide();

                    return wb.Deliver("Properties List -" + DateTime.Now.ToShortDateString() + ".xlsx");

                    // or specify the content type:
                    //  return wb.Deliver("generatedFile.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }
            }

        }


        #region RequisitionRequest 




        //public ActionResult RequisitionRequest()
        //{
        //    return View(new BaseViewModel());
        //}

        public async Task<ActionResult> PrintRequisitionRequest(string id)
        {
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetRequisitionRequest?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintRequisitionRequest>(EmpResponse);

                }
                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "RequisitionRequest");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                SiteUser user = new SiteUser();
                if (Session["user"] != null)
                {
                    user = (SiteUser)Session["user"];
                }

                WorkflowHelper.InsertHrLog("RequisitionRequest", id, "Print has been requested ", "Print has been requested for Requisition Request on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));

                // o.Date=obj.
                //returning the employee list to view  
                return View("PrintRequisitionRequest", obj);
            }

        }

        public async Task<ActionResult> PdfRequisitionRequest(string id)
        {

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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetRequisitionRequest?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintRequisitionRequest>(EmpResponse);

                }
                HttpResponseMessage Res1 = await client.GetAsync("api/data/GetHRFormsActivityLogExport?Id=" + id + "&FormType=" + "RequisitionRequest");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj.printActivity = JsonConvert.DeserializeObject<PrintActivity>(EmpResponse);

                }

                SiteUser user = new SiteUser();
                if (Session["user"] != null)
                {
                    user = (SiteUser)Session["user"];
                }


                WorkflowHelper.InsertHrLog("RequisitionRequest", id, "PDF has been requested", "PDF has been requestedfor Requisition Request on" + DateTime.Now, FirstCharToUpper(user.FirstName)+ " "+ FirstCharToUpper(user.LastName));


                // o.Date=obj.
                //returning the employee list to view  
                //      return View(obj);


                //returning the employee list to view  
                return new ViewAsPdf("PrintRequisitionRequest", obj) { PageSize = Size.A4, CustomSwitches = "--disable-smart-shrinking", FileName = "RequisitionRequest-"+ obj.SequenceNumber + "-" + obj.RequestorName + ".pdf" };

            }

           

        }
        public static string FirstCharToUpper(string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        #endregion


    }
}