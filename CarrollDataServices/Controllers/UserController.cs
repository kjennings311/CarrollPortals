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

    //  [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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


        [CustomActionFilter]
        [ActionName("GetRolesForSelect")]
        public List<KeyValuePair> GetRolesForSelect()
        {
            List<KeyValuePair> _roles = new List<KeyValuePair>();
            //us

            foreach (var item in _service.GetAllRoles())
            {
                _roles.Add(new KeyValuePair(item.RoleId.ToString(), item.RoleName.ToString()));
            }
            return _roles;
        }


        [CustomActionFilter]
        [ActionName("GetPositionTypesForSelect")]
        public List<KeyValuePair> GetPositionTypesForSelect()
        {
            List<KeyValuePair> _roles = new List<KeyValuePair>();
            //us
                _roles.Add(new KeyValuePair("Corporate", "Corporate"));
            _roles.Add(new KeyValuePair("Property", "Property"));

            return _roles;
        }




        [CustomActionFilter]
        [ActionName("GetUsersForSelect")]
        public List<KeyValuePair> GetUsersForSelect()
        {
            List<KeyValuePair> _users = new List<KeyValuePair>();
            //us

            foreach (var item in _service.GetAllUsers())
            {
                _users.Add(new KeyValuePair(item.UserId.ToString(), item.UserEmail + "( " + item.FirstName + " " + item.LastName + " )"));
            }
            return _users;

        }

        [CustomActionFilter]
        [ActionName("GetPropertiesForSelect")]
        public List<KeyValuePair> GetPropertiesForSelect()
        {
            List<KeyValuePair> _users = new List<KeyValuePair>();
            //us

            foreach (var item in _service.GetAllProperties())
            {
                _users.Add(new KeyValuePair(item.PropertyId.ToString(), item.PropertyName));
            }
            return _users;

        }


        [CustomActionFilter]
        [ActionName("GetRoles")]
        public List<Carroll.Data.Entities.Role> GetRoles()
        {
            return _service.GetAllRoles();
        }



        [HttpGet]        
        //[ActionName("CheckIfUserExists")]
        public bool CheckIfUserExists(string id)
        {
           return _service.CheckIfUserExists(id);
            
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


        public bool AuthenticateUser(string User, string Password)
        {
            return _service.AuthenticateUser(User, Password);
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