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
using Newtonsoft.Json;
using Rotativa;


namespace Carroll.Portals.Controllers
{
    [CustomAuthorize]
    [BaseModel]
    [AdminHRFilter]
    public class MileageController : Controller
    {
        string Baseurl = "";

        public MileageController()
        {
            Baseurl = ConfigurationManager.AppSettings["ServiceURL"];
        }

        // GET: CarrollMileageLog
        public ActionResult CarrollMileageLog()
        {
            return View(new BaseViewModel());
        }

        // GET: Hr
        public async Task<ActionResult> PrintMonthlyLog(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetMileageLog?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintMileageLog>(EmpResponse);

                }

                // o.Date=obj.
                //returning the employee list to view  
                return View(obj);
            }

        }

        public async Task<ActionResult> PdfMonthlyMileageLog(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetMileageLog?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintMileageLog>(EmpResponse);

                }

                // o.Date=obj.
                //returning the employee list to view  
                return new ViewAsPdf("PrintMonthlyLog", obj) { FileName = "MonthlyMileageLog - " + DateTime.Now.ToShortDateString() + ".pdf" };

            }

        }


        // GET: CarrollMileageLogInstructions
        public ActionResult CarrollMileageLogInstructions()
        {
            return View(new BaseViewModel());
        }
        // GET: CarrollMileageLogInstructions
        public ActionResult CarrollExpenseForm()
        {
            return View(new BaseViewModel());
        }

        // GET: Hr
        public async Task<ActionResult> PrintExpenseReimbursement(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetExpenseReimbursement?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintExpenseForm>(EmpResponse);

                }

                // o.Date=obj.
                //returning the employee list to view  
                return View(obj);
            }

        }

        public async Task<ActionResult> PdfExpenseReimbursement(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetExpenseReimbursement?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintExpenseForm>(EmpResponse);

                }

                // o.Date=obj.
                //returning the employee list to view  
                return new ViewAsPdf("PrintExpenseReimbursement", obj) { FileName = "ExpenseForm - " + DateTime.Now.ToShortDateString() + ".pdf" };

            }

        }



    }
}