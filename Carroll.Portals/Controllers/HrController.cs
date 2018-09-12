using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Carroll.Portals.Helpers;
using Carroll.Portals.Models;
using Newtonsoft.Json;
using Rotativa;

namespace Carroll.Portals.Controllers
{
    [CustomAuthorize]
    [BaseModel]
    public class HrController : Controller
    {
        string Baseurl = "";

        public HrController()
        {
            Baseurl = "http://localhost:1002/";
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
                return new ViewAsPdf("PrintEmployeeLeaseRider",obj) { FileName = "EmployeeLeaseRider - " + DateTime.Now.ToShortDateString() + ".pdf" };
                
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


    }
}