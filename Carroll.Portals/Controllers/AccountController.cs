using Carroll.Data.Services.Models;
using System.Web.Mvc;

using Carroll.Portals.Models;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Services.Models.Validation;

namespace Carroll.Portals.Controllers
{
    public class AccountController : Controller
    {
        private IDataService _service;
        private IValidationDictionary _modelState;

        public AccountController()
        {
            _modelState = new ModelStateWrapper(this.ModelState);
            _service = new DataService(_modelState, new EntityDataRepository());

        }

        public AccountController(IDataService service)
        {
            _service = service;
        }
       

        [AllowAnonymous]
        [HttpGet]
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: Account
        public ActionResult CheckLogin(LoginModel Login)
        {
            if(ModelState.IsValid)
            {
               
            }
            else
            {
                // show error message
                ModelState.AddModelError("Error", "Invalid Username or Password");
            }
            return View("Login");
}

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
    }
}