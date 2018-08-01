using Carroll.Portals.Helpers;
using Carroll.Portals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carroll.Portals.Controllers
{
    [CustomAuthorize]
    [BaseModel]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Forms()
        {
            return View(new BaseViewModel());
        }
    }
}