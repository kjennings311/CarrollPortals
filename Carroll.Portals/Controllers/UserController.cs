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
    [AdminHRFilter]


    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View(new BaseViewModel());
        }
        // GET: User
        public ActionResult UserRoles()
        {
            return View(new BaseViewModel());
        }
        // GET: User
        public ActionResult UserProperties()
        {
            return View(new BaseViewModel());
        }

        // GET: User
        public ActionResult CarrollPositions()
        {
            return View(new BaseViewModel());
        }


        // GET: User
        public ActionResult PayPeriods()
        {
            return View(new BaseViewModel());
        }

        // GET: User
        public ActionResult ErrorLog()
        {
            return View(new BaseViewModel());
        }

    }
}