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
namespace Carroll.Portals.Controllers
{
    [CustomAuthorize]
    [BaseModel]
    [AdminHRFilter]
    public class HrController : Controller
    {

        string Baseurl = "";

        public HrController()
        {
            Baseurl = ConfigurationManager.AppSettings["ServiceURL"];
        }

        public ActionResult Index()
        {
            return View(new BaseViewModel());
        }

        // GET: Hr
        public async Task<ActionResult> PrintEmployeeLeaseRider(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid=" + id);
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
                return View(obj);
            }

        }

        public async Task<ActionResult> PdfEmployeeLeaseRider(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid=" + id);
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
                return new ViewAsPdf("PrintEmployeeLeaseRider", obj) { FileName = "EmployeeLeaseRider - " + DateTime.Now.ToShortDateString() + ".pdf" };

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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeNewHireNotice?riderid=" + id);
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
                return View("PrintEmployeeNewHireNotice", obj);
            }

        }

        public async Task<ActionResult> PdfEmployeeNewHireNotice(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeNewHireNotice?riderid=" + id);
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
                //      return View(obj);


                //returning the employee list to view  
                return new ViewAsPdf("PrintEmployeeNewHireNotice", obj) { FileName = "EmployeeNewHireNotice - " + DateTime.Now.ToShortDateString() + ".pdf" };

            }
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

            var _service = new EntityDataRepository();
            var retu = _service.UpdateWorkflowEmployeeNewHireNotice(action, refid, signature, edate);

            if (action == "Employee Email" && iscorporate.ToLower() == "false")
            {
                WorkflowHelper.SendHrWorkFlowEmail(retu, "NewHire", "Regional Email");
            }
            else
            {
               var Message= WorkflowHelper.SendHrWorkFlowEmail(retu, "NewHire", "Manager Email");

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

                    mail.From = new MailAddress("sekharbabu101@gmail.com", "Carroll Organization");

                
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



                    var actionPDF = new Rotativa.ViewAsPdf("PrintEmployeeNewHireNotice", obj)//some route values)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
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
                    mail.To.Clear();
                    // remove this line before going production
                    //  mail.To.Add("pavan.nanduri@carrollorg.com");
                    mail.To.Add("sekhar.babu@forcitude.com");
                     mail.To.Add("Shashank.Trivedi@carrollorg.com");

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
            
            var Message = WorkflowHelper.ReSendHrWorkFlowEmail(refid,form,"");

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

                mail.From = new MailAddress("sekharbabu101@gmail.com", "Carroll Organization");


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

                                var actionPDF = new Rotativa.ViewAsPdf("PrintEmployeeLeaseRider", obj)//some route values)
                                {
                                    //FileName = "TestView.pdf",
                                    PageSize = Size.A4,
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



                    var actionPDF = new Rotativa.ViewAsPdf("PrintNoticeOfEmployeeSeparation", obj)//some route values)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Size.A4,
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



                    var actionPDF = new Rotativa.ViewAsPdf("PrintRequisitionRequest", obj)//some route values)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Size.A4,
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
                mail.To.Clear();
                // remove this line before going production
                //  mail.To.Add("pavan.nanduri@carrollorg.com");
                mail.To.Add("sekhar.babu@forcitude.com");
                  mail.To.Add("Shashank.Trivedi@carrollorg.com");

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

                // o.Date=obj.
                //returning the employee list to view  
                //      return View(obj);


                //returning the employee list to view  
                return new ViewAsPdf("PrintPayRollStatusChange", obj) { FileName = "PayROll Status Change - " + DateTime.Now.ToShortDateString() + ".pdf" };

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

                // o.Date=obj.
                //returning the employee list to view  
                //      return View(obj);


                //returning the employee list to view  
                return new ViewAsPdf("PrintNoticeOfEmployeeSeparation", obj) { FileName = "NoticeOfEmployeeSeparation - " + DateTime.Now.ToShortDateString() + ".pdf" };

            }

        }

        #endregion


        #region RequisitionRequest 

        public ActionResult RequisitionRequest()
        {
            return View(new BaseViewModel());
        }

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

                // o.Date=obj.
                //returning the employee list to view  
                //      return View(obj);


                //returning the employee list to view  
                return new ViewAsPdf("PrintRequisitionRequest", obj) { FileName = "RequisitionRequest - " + DateTime.Now.ToShortDateString() + ".pdf" };

            }

        }

        #endregion

        
    }
}