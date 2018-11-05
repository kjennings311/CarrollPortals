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

namespace Carroll.Portals.Controllers
{
    [CustomAuthorize]
    [BaseModel]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new BaseViewModel());
        }


        public ActionResult Help()
        {
            return View(new BaseViewModel());
        }


        public ActionResult ViewClaim(string Claim)
        {
            return View(new BaseViewModel());
        }


        //public ActionResult PrintClaim(string Claim)
        //{
        //    return View(new BaseViewModel());
        //}

        public async Task<ActionResult> PrintClaim(string Claim, char Type)
        {

            string Baseurl = ConfigurationManager.AppSettings["ServiceURL"];

            dynamic obj = new { };

            PrintViewClaim n = new PrintViewClaim();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                //   HttpResponseMessage Res = await client.GetAsync("api/data/GetEmployeeLeaseRider?riderid="+id);
                HttpResponseMessage Res = await client.GetAsync("api/data/GetClaimDetailsForPrint?Claim=" + Claim + "&Type=" + Type);
                //Checking the response is successful or not which is sent using HttpClient  
                string Propid = "";

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    n.Type = Type;

                    if (Type == 'g')
                    {
                        n.GLC = JsonConvert.DeserializeObject<PrintGeneralLiabilityClaim>(EmpResponse);
                        Propid =n.GLC.PropertyId.ToString();
                    }
                    else if (Type == 'm')
                    {
                        n.MDC = JsonConvert.DeserializeObject<PrintMoldDamageClaim>(EmpResponse);
                        Propid = n.MDC.PropertyId.ToString();
                    }
                    else if (Type == 'p')
                    {
                        n.PDC = JsonConvert.DeserializeObject<PrintPropertyDamageClaim>(EmpResponse);
                        Propid = n.PDC.PropertyId.ToString();
                    }

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    
                }


                HttpResponseMessage resp = await client.GetAsync("api/data/GetUserPropertyForClaimPrint/?userid=" + Propid);
                //Checking the response is successful or not which is sent using HttpClient  
                if (resp.IsSuccessStatusCode)
                {

                    //Storing the response details recieved from web api   
                    var PropResponse = resp.Content.ReadAsStringAsync().Result;

                  var propres=JsonConvert.DeserializeObject<PrintProperty>(PropResponse);

                    n.Prop = propres;

                }
                    // o.Date=obj.
                    //returning the employee list to view  
                    return View(n);
            }


            //}
        }
    }
}