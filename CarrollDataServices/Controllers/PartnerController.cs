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
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using System.Security.Claims;
using System.Web.Http.Cors;
using Carroll.Data.Services.Models.Partners;

namespace Carroll.Data.Services.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PartnerController : ApiController
    {
        private IPartnerService _service;
        private IValidationDictionary _modelState;

        public PartnerController()
        {
            _modelState = new ModelStateWrapper(this.ModelState);
            _service = new PartnerService(_modelState, new EntityPartnerRepository());

        }

        public PartnerController(IPartnerService service)
        {
            _service = service;
        }

        [ActionName("GetTokenInputUser")]
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<TokenInput> GetTokenInputUser(string q)
        {

            var _users = _service.GetAllPartners(q);
            List<TokenInput> _coll = new List<TokenInput>();
            foreach (EquityPartner _user in _users)
            {
                TokenInput _input = new TokenInput();
                _input.id = _user.ContactId.ToString();
                _input.name = _user.PartnerName ;
                _input.ReadOnly = false;
                _coll.Add(_input);
            }
            return _coll;
        }

        [ActionName("GetAllPartners")]
        [AllowAnonymous]
        [HttpGet]
        public List<EquityPartner> GetAllPartners()
        {
            return _service.GetAllPartners();
        }
    }
}