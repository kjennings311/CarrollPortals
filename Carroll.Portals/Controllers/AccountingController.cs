using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carroll.Portals.Helpers;
using Carroll.Portals.Models;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rotativa;
namespace Carroll.Portals.Controllers
{
    [CustomAuthorize]
    [BaseModel]
    [AdminHRFilter]
    public class AccountingController : Controller
    {
        string Baseurl = "";

        public AccountingController()
        {
            Baseurl = ConfigurationManager.AppSettings["ServiceURL"];
        }

        // GET: Accounting
        public ActionResult MoveOutCoverSheet()
        {
            return View(new BaseViewModel());
        }

        // GET: Accounting
        public ActionResult PettyCashPolicy()
        {
            return View(new BaseViewModel());
        }

        public ActionResult ReclassandAccrual()
        {
            return View(new BaseViewModel());
        }


        public ActionResult EomChecklist()
        {
            return View(new BaseViewModel());
        }
    }
}