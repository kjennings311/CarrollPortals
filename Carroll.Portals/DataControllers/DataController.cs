using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carroll.Portals.Helpers;
using Carroll.Portals.Models;
namespace Carroll.Portals.Controllers
{
    [CustomAuthorize]
    [BaseModel]
    public class DataController : Controller
    {
        // GET: Form
        [ActionName("PropertyDamageClaim")]
        public ActionResult PropertyDamageClaim()
        {
            return View(new BaseViewModel());
        }

        [ActionName("GeneralLiabilityClaim")]
        public ActionResult GeneralLiabilityClaim()
        {
            return View(new BaseViewModel());
        }

        [ActionName("Properties")]
        public ActionResult Properties()
        {
            return View(new BaseViewModel());
        }
        [ActionName("AddProperty")]
        public ActionResult AddProperty()
        {
            return View(new BaseViewModel());
        }

        [ActionName("Contacts")]
        public ActionResult Contacts()
        {
            return View(new BaseViewModel());
        }

        [ActionName("Partners")]
        public ActionResult Partners()
        {
            return View(new BaseViewModel());
        }
    }
}
