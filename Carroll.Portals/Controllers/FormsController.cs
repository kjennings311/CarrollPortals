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
    public class FormsController : Controller
    {
        // GET: Forms
        public ActionResult GeneralLiabilityClaims()
        {
            return View(new BaseViewModel());
        }
        // GET: Forms
        public ActionResult MoldDamageClaims()
        {
            return View(new BaseViewModel());
        }
        // GET: Forms
        public ActionResult PropertyDamageClaims()
        {
            return View(new BaseViewModel());
        }
    }
}