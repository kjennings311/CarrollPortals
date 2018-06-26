using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carroll.Portals.Controllers
{
    //[Authorize]
    public class DataController : Controller
    {
        // GET: Form
        [ActionName("PropertyDamageClaim")]
        public ActionResult PropertyDamageClaim()
        {
            return View();
        }

        [ActionName("GeneralLiabilityClaim")]
        public ActionResult GeneralLiabilityClaim()
        {
            return View();
        }

        [ActionName("Properties")]
        public ActionResult Properties()
        {
            return View();
        }
        [ActionName("AddProperty")]
        public ActionResult AddProperty()
        {
            return View();
        }

        [ActionName("Contacts")]
        public ActionResult Contacts()
        {
            return View();
        }

        [ActionName("Partners")]
        public ActionResult Partners()
        {
            return View();
        }


    }
}
