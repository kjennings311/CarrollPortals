using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carroll.Data.Entities;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Text;
using System.Web.Script.Serialization;
using System.Net.Http.Headers;
using Carroll.Data.Services.Models.Validation;
using Carroll.Data.Services;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Services.Models.Properties;
using System.Web.Http.Cors;
using Carroll.Data.Services.Helpers;

namespace Carroll.Data.Services.Controllers
{
    //[Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PropertyController : ApiController   {

        private IPropertyService _service;
        private IValidationDictionary _modelState;
        private System.Web.Http.ModelBinding.ModelStateDictionary _modelstate = new System.Web.Http.ModelBinding.ModelStateDictionary();
        public PropertyController()
        {
            _modelState = new ModelStateWrapper(this.ModelState);
            _service = new PropertyService(_modelState, new EntityPropertyRepository());

        }

        public PropertyController(IPropertyService service)
        {
            _service = service;
        }

        [ActionName("CUProperty")]
        [HttpPost]
        public HttpResponseMessage CUProperty([FromBody]Property FormData)
        {
            //string sCurrentUser = User.Identity.Name;
            //FormData.ModifiedBy = sCurrentUser;
            //// set it here.. repository will ignore if it is an update.
            //FormData.CreatedBy = sCurrentUser;
            ////  FormData.UserName = sCurrentUser;

            Property _prop = FormData as Property;

            bool bSucceded = _service.CreateUpdateProperty(FormData);
            return Utility.ReturnRecordResponse(_modelState, bSucceded);
            //return bSucceded;
        }

        [ActionName("DeleteProperty")]
        [HttpGet]
        public bool DeleteProperty(string PropertyId)
        {

            if (!string.IsNullOrEmpty(PropertyId))
            {
                return _service.DeleteProperty(PropertyId);
            }
            else return false;
        
        }

        [ActionName("GetProperties")]
        [HttpGet]
        public List<spProperties_Result> GetProperties()
        {
            return _service.GetProperties();
        }

        [ActionName("GetProperty")]
        [HttpGet]
        public List<spProperties_Result> GetProperty(string id)
        {
            return _service.GetProperties(id);
        }


    }
}