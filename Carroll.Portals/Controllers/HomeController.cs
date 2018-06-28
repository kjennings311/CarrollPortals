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
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new BaseViewModel());
        }
    }
}