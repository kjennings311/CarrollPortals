﻿using System;
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
    [AdminFilter]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View(new BaseViewModel());
        }
    }
}