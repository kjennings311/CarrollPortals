using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    }
}