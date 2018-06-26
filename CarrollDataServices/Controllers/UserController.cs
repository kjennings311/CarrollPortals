using Carroll.Data.Services.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Carroll.Data.Services.Models.Validation;
using Carroll.Data.Services.Helpers;
using System.Web.Http.ModelBinding;
using System.Text;
using System.Web.Script.Serialization;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Carroll.Data.Services.Models.Users;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using System.Security.Claims;
using System.Web.Http.Cors;

namespace Carroll.Data.Services.Controllers
{
    
    [Authorize]
    public class UserController : ApiController
    {
        private IUserService _service;
        private IValidationDictionary _modelState;
    
        public UserController()
        {
            _modelState = new ModelStateWrapper(this.ModelState);
            _service = new UserService(_modelState, new EntityUserRepository());

        }

        public UserController(IUserService service)
        {
            _service = service;
        }

        [CustomActionFilter]
        [ActionName("GetUsers")]
        public List<SiteUser> GetUsers()
        {
            
                return _service.GetAllUsers();           
        }
        
        [ActionName("CheckIfUserExists")]
        public bool CheckIfUserExists(string id)
        {
            return false;
        }

        [HttpPost]
        public bool CreateUser([FromBody] SiteUser user)
        {
            return _service.CreateUser(user);
        }

        /// <summary>
        /// Function checks if user logging in is authenticated and has a profile in the App
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("EnsureUserHasProfile")]
        [HttpPost]
        public bool EnsureUserHasProfile([FromBody] SiteUser user)
        {

          //   var _user = user;
            return _service.CreateUser(user);
           

        }
        [ActionName("GetUser")]
        [HttpGet]
        public SiteUser  GetUser(string EmailOrGuid)
        {                        
            //   var _user = user;
            return _service.GetUser(EmailOrGuid);

        }




        //[ActionName("GetTokenInputUser")]
        //[AllowAnonymous]
        //[HttpGet]
        //public IEnumerable<TokenInput> GetTokenInputUser(string q)
        //{

        //    var _users = _service.GetAllContacts(q);
        //    List<TokenInput> _coll = new List<TokenInput>();
        //    foreach (Contact _user in _users)
        //    {
        //        TokenInput _input = new TokenInput();
        //        _input.id = _user.ContactId.ToString();
        //        _input.name = _user.FirstName + " " + _user.LastName + "<br/>" + _user.Email;
        //        _input.ReadOnly = false;
        //        _coll.Add(_input);
        //    }
        //    return _coll;
        //}

        //[ActionName("GetAllContacts")]
        //public List<Contact> GetAllContacts()
        //{
        //    return _service.GetAllContacts();
        //}
    }
}