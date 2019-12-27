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
    public class ResidentController : Controller
    {

        string Baseurl = "";

        public ResidentController()
        {
            Baseurl = ConfigurationManager.AppSettings["ServiceURL"];
        }

        public ActionResult ReferalRequest()
        {
            return View(new BaseViewModel());
        }

        // GET: Hr
        public async Task<ActionResult> Printresidentreferal(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetResidentReferralRequest?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintResidentReferralSheet>(EmpResponse);

                }

                // o.Date=obj.
                //returning the employee list to view  
                return View(obj);
            }

        }

        public async Task<ActionResult> PdfResidentReferal(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetResidentReferralRequest?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<PrintResidentReferralSheet>(EmpResponse);

                }

                // o.Date=obj.
                //returning the employee list to view  
                return new ViewAsPdf("Printresidentreferal", obj) { FileName = "ResidentReferralSheet - " + DateTime.Now.ToShortDateString() + ".pdf" };

            }

        }

        public ActionResult ReferalContact()
        {
            return View(new BaseViewModel());
        }

        public ActionResult EmployeeNewHireNotice()
        {
            return View(new BaseViewModel());
        }

        public async Task<ActionResult> Printresidentcontact(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetResidentReferralContact?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<Carroll.Data.Entities.Repository.PrintResidentContact>(EmpResponse);

                }

                // o.Date=obj.
                //returning the employee list to view  
                return View(obj);
            }
        }

        public async Task<ActionResult> PdfResidentContact(string id)
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
                HttpResponseMessage Res = await client.GetAsync("api/data/GetResidentReferralContact?riderid=" + id);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    obj = JsonConvert.DeserializeObject<Carroll.Data.Entities.Repository.PrintResidentContact>(EmpResponse);

                }

                // o.Date=obj.
                //returning the employee list to view  
                return new ViewAsPdf("Printresidentcontact", obj) { FileName = "ResidentReferralContact - " + DateTime.Now.ToShortDateString() + ".pdf" };

            }

        }
    }
}