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

namespace Carroll.Portals.Controllers
{

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

    }
}