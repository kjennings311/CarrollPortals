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


namespace Carroll.Data.Services.Controllers
{
   //  [EnableCors(origins = new[] { "http://localhost", "http://sample.com" })]
      [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
    //[RoutePrefix("api/Data")]
    // [Authorize]
   // [EnableCors(origins: "http://localhost")]
    public class DataController : ApiController
    {
        private IDataService _service;
        private IValidationDictionary _modelState;

        public DataController()
        {
            _modelState = new ModelStateWrapper(this.ModelState);
            _service = new DataService(_modelState, new EntityDataRepository());

        }

        public DataController(IDataService service)
        {
            _service = service;
        }
        //[CustomActionFilter]
        //[ActionName("GetUsers")]
        //public List<SiteUser> GetUsers()
        //{

        //    return _service.GetAllUsers();
        //}


        //[ActionName("GetRecords")]
        //[HttpGet]
        //public string GetRecords(EntityType entityType)
        //{
          
        //        return new JavaScriptSerializer().Serialize(_service.GetRecords(entityType, ""));
          
          
        //}

        //**************************************************************************Record******************************************************//
        [ActionName("GetRecords")]
        [HttpGet]
        public dynamic GetRecords(EntityType entityType, string optionalText="")
        {

            return _service.GetRecords(entityType, optionalText);

        }

        [ActionName("GetRecordsWithConfig")]
        [HttpGet]
        public dynamic GetRecordsWithConfig(EntityType entityType, string optionalText = "")
        {
            return _service.GetRecordsWithConfig(entityType, optionalText);

        }

        [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
        [ActionName("DeleteRecord")]
        [HttpPost]
        
        public bool DeleteRecord(EntityType entityType, string recordId)
        {

            if (!string.IsNullOrEmpty(entityType.ToString()) && (!string.IsNullOrEmpty(recordId)))
            {
                return _service.DeleteRecord(entityType,recordId);
            }
            else return false;

        }
        //**************************************************************************Record******************************************************//
        [ActionName("GetTokenInputUser")]
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<TokenInput> GetTokenInputUser(string q)
        {

            var _users = _service.GetRecords(EntityType.Contact,q);
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






        [ActionName("GetStates")]
        [HttpGet]
        [AllowAnonymous]
        public List<KeyValuePair> GetStates()
        {
            List<KeyValuePair> _states = new List<KeyValuePair> {
              //us
              new KeyValuePair("AL", "Alabama"),
              new KeyValuePair("AK", "Alaska"),
              new KeyValuePair("AZ", "Arizona"),
              new KeyValuePair("AR", "Arkansas"),
              new KeyValuePair("CA", "California"),
              new KeyValuePair("CO", "Colorado"),
              new KeyValuePair("CT", "Connecticut"),
              new KeyValuePair("DE", "Delaware"),
              new KeyValuePair("DC", "District Of Columbia"),
              new KeyValuePair("FL", "Florida"),
              new KeyValuePair("GA", "Georgia"),
              new KeyValuePair("HI", "Hawaii"),
              new KeyValuePair("ID", "Idaho"),
              new KeyValuePair("IL", "Illinois"),
              new KeyValuePair("IN", "Indiana"),
              new KeyValuePair("IA", "Iowa"),
              new KeyValuePair("KS", "Kansas"),
              new KeyValuePair("KY", "Kentucky"),
              new KeyValuePair("LA", "Louisiana"),
              new KeyValuePair("ME", "Maine"),
              new KeyValuePair("MD", "Maryland"),
              new KeyValuePair("MA", "Massachusetts"),
              new KeyValuePair("MI", "Michigan"),
              new KeyValuePair("MN", "Minnesota"),
              new KeyValuePair("MS", "Mississippi"),
              new KeyValuePair("MO", "Missouri"),
              new KeyValuePair("MT", "Montana"),
              new KeyValuePair("NE", "Nebraska"),
              new KeyValuePair("NV", "Nevada"),
              new KeyValuePair("NH", "New Hampshire"),
              new KeyValuePair("NJ", "New Jersey"),
              new KeyValuePair("NM", "New Mexico"),
              new KeyValuePair("NY", "New York"),
              new KeyValuePair("NC", "North Carolina"),
              new KeyValuePair("ND", "North Dakota"),
              new KeyValuePair("OH", "Ohio"),
              new KeyValuePair("OK", "Oklahoma"),
              new KeyValuePair("OR", "Oregon"),
              new KeyValuePair("PA", "Pennsylvania"),
              new KeyValuePair("RI", "Rhode Island"),
              new KeyValuePair("SC", "South Carolina"),
              new KeyValuePair("SD", "South Dakota"),
              new KeyValuePair("TN", "Tennessee"),
              new KeyValuePair("TX", "Texas"),
              new KeyValuePair("UT", "Utah"),
              new KeyValuePair("VT", "Vermont"),
              new KeyValuePair("VA", "Virginia"),
              new KeyValuePair("WA", "Washington"),
              new KeyValuePair("WV", "West Virginia"),
              new KeyValuePair("WI", "Wisconsin"),
              new KeyValuePair("WY", "Wyoming")
              ////canada
              //new KeyValuePair("AB", "Alberta"),
              //new KeyValuePair("BC", "British Columbia"),
              //new KeyValuePair("MB", "Manitoba"),
              //new KeyValuePair("NB", "New Brunswick"),
              //new KeyValuePair("NL", "Newfoundland and Labrador"),
              //new KeyValuePair("NS", "Nova Scotia"),
              //new KeyValuePair("NT", "Northwest Territories"),
              //new KeyValuePair("NU", "Nunavut"),
              //new KeyValuePair("ON", "Ontario"),
              //new KeyValuePair("PE", "Prince Edward Island"),
              //new KeyValuePair("QC", "Quebec"),
              //new KeyValuePair("SK", "Saskatchewan"),
              //new KeyValuePair("YT", "Yukon"),
      };
            return _states;


        }

        [ActionName("GetEquityPartners")]
        [HttpGet]
        [AllowAnonymous]
        public List<KeyValuePair> GetEquityPartners()
        {
            var _partners = _service.GetRecords(EntityType.Partner);
            List<KeyValuePair> _equityPartners = new List<KeyValuePair>();
            foreach(EquityPartner _partner in _partners)
            {
                _equityPartners.Add(new KeyValuePair(_partner.EquityPartnerId.ToString(), _partner.PartnerName));
            }

            _partners = null;

              //us



            return _equityPartners;


        }
    }
}
