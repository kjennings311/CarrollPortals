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
using Carroll.Data.Services.Models.Contact;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using System.Security.Claims;
using System.Web.Http.Cors;

namespace Carroll.Data.Services.Controllers
{
    [Authorize]
    public class ContactController : ApiController
    {
        private IContactService _service;
        private IValidationDictionary _modelState;

        public ContactController()
        {
            _modelState = new ModelStateWrapper(this.ModelState);
            _service = new ContactService(_modelState, new EntityContactRepository());

        }

        public ContactController(IContactService service)
        {
            _service = service;
        }

        [ActionName("GetTokenInputUser")]
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<TokenInput> GetTokenInputUser(string q)
        {

            var _users = _service.GetAllContacts(q);
            List<TokenInput> _coll = new List<TokenInput>();
            foreach (Contact _user in _users)
            {
                TokenInput _input = new TokenInput();
                _input.id = _user.ContactId.ToString();
                _input.name = _user.FirstName + " " + _user.LastName + "<br/>" + _user.Email;
                _input.ReadOnly = false;
                _coll.Add(_input);
            }
            return _coll;
        }

        [ActionName("GetAllContacts")]
        [AllowAnonymous]
        [HttpGet]
        public List<Contact> GetAllContacts()
        {
            return _service.GetAllContacts();
        }

        [ActionName("GetContact")]
        [AllowAnonymous]
        [HttpGet]
        public Contact GetContact(string ContactId)
        {
            return _service.GetContact(ContactId);
        }
    }
}