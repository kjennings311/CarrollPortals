using Carroll.Data.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Carroll.Data.Services.Helpers;
using Carroll.Data.Services.Models.Validation;
using Carroll.Data.Entities.Repository;
//using Carroll.Data.Services.Models.Properties;
//using Carroll.Data.Services.Models.Contact;
using Carroll.Data.Entities;
using System.Reflection;

namespace Carroll.Data.Services.Controllers
{
    //[Authorize]
    public class FormController : ApiController
    {

        private IDataService _service;
        private IValidationDictionary _modelState;
        private System.Web.Http.ModelBinding.ModelStateDictionary _modelstate = new System.Web.Http.ModelBinding.ModelStateDictionary();
        public FormController()
        {
            _modelState = new ModelStateWrapper(this.ModelState);
            _service = new DataService(_modelState, new EntityDataRepository());

        }

        public FormController(IDataService service)
        {
            _service = service;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Where id is the formname Property, Contact, Partner EntiType value</param>
        /// <returns></returns>
        [ActionName("GenerateForm")]
        [HttpGet]
        public Form GenerateForm(string id)
        {
            string FormName = id;
            try
            {
                string xmlPath = "";
                if (string.IsNullOrEmpty(FormName)) return null;
                xmlPath = string.Concat("~/ModelXml/", FormName.ToLower(), ".xml");
                //switch (FormName.ToLower())
                //{
                //    case "property":
                //        xmlPath = "~/ModelXml/property.xml";
                //        break;
                //    case "contact":
                //        xmlPath = "~/ModelXml/property.xml";
                //        break;
                //    default:
                //        xmlPath = "~/ModelXml/property.xml";
                //        break;
                //}
                var xmlFullPath = System.Web.Hosting.HostingEnvironment.MapPath(@xmlPath);
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(xmlFullPath);


                Form _form = Utility.DeserializeObject<Form>(doc.AsString());

                return _form;
            }
            catch(Exception ex) { string s = ex.ToString(); return null; }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Where id is the formname Property, Contact, Partner EntiType value</param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        
        [ActionName("GenerateEditForm")]
        [HttpGet]
        public Form GenerateEditForm(EntityType entityType, string recordId)
        {
            // so the url can be /api/data/?Formname=xxx&recordid=xxx
          
            try
            {
                string xmlPath = "";
            //    if (string.IsNullOrEmpty(FormName.GetType().ToString())) return null;
                xmlPath = string.Concat("~/ModelXml/", entityType.ToString(), ".xml");
               
                var xmlFullPath = System.Web.Hosting.HostingEnvironment.MapPath(@xmlPath);
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(xmlFullPath);


                Form _form = Utility.DeserializeObject<Form>(doc.AsString());
                IValidationDictionary _modelState = new ModelStateWrapper(this.ModelState);


                var _data = _service.GetRecord(entityType, recordId);
                if (_data != null)
                {
                    // now that we have the form let's populate the form fields
                    _form.IsEditForm = true;
                    _form.DBFieldId = recordId;
                    foreach (FormField _ff in _form.FormFields)
                    {
                        _ff.FieldValue = Convert.ToString(Utility.GetPropertyValue(_data, _ff.FieldName));
                    }

                    return _form;

                }
                #region commented
                // now that we have the form.. Let's identify what type of form it is and get record based on the form
                //switch (FormName.ToLower())
                //{
                //    case "property":
                //        // we know it is a property form.. Let's get related property record for the given recordid

                //        // Carroll.Data.Services.Models.Properties.IDA _service = new Carroll.Data.Services.Models.Properties.PropertyService(_modelState, new EntityPropertyRepository());
                //        Property _property = _service.GetRecord(EntityType.Property, RecordId);

                //        break;
                //    case "contact":
                //        // we know it is a property form.. Let's get related property record for the given recordid

                //        //Carroll.Data.Services.Models.Contact.IContactService _contactService = new Carroll.Data.Services.Models.Contact.ContactService(_modelState, new EntityContactRepository());
                //        Contact _contact = _Service.GetContact(EntityType RecordId);
                //        if (_contact != null)
                //        {
                //            // now that we have the form let's populate the form fields
                //            _form.IsEditForm = true;
                //            _form.DBFieldId = RecordId;
                //            foreach (FormField _ff in _form.FormFields)
                //            {
                //                _ff.FieldValue = Convert.ToString(Utility.GetPropertyValue(_contact, _ff.FieldName));
                //            }

                //            return _form;

                //        }
                //        break;
                //    default:
                //        break;

                //}
                #endregion

                return _form;
            }
            catch {return null; }


        }

        #region [ CRUD OPERATIONS ]

        [ActionName("CreateUpdateFormData")]
        [HttpPost]
        public HttpResponseMessage CreateUpdateFormData(EntityType id,[FromBody]dynamic JsonData)
        {
            IValidationDictionary _modelState = new ModelStateWrapper(this.ModelState);
            Form _formdata = JsonData.ToObject<Form>();
            bool bSucceeded = false;
            // Validate the data that is sent.. 

            if (Utility.ValidateFormData(_modelState, _formdata.FormFields))
            {
                // Now data is valid let's pass to DAL for update/Insert

                object obj = _service.GetRuntimeClassInstance(_formdata.FormName);

                foreach (FormField _field in _formdata.FormFields)
                {
                    obj.SetPropertyValue(_field.FieldName, _field.FieldValue);
                }
            
                bSucceeded = _service.CreateUpdateRecord(id, obj);
               return Utility.ReturnRecordResponse(_modelState, bSucceeded);

             
            }
            else
            {
                return Utility.ReturnRecordResponse(_modelState, false);
            }

          //  return null;

        }


        //[ActionName("DeleteRecord")]
        //[HttpPost]
        //// WHERE ID
        //public bool DeleteRecord(EntityType formName, string recordId)
        //{
            
        //    if (!string.IsNullOrEmpty(recordId))
        //    {
        //        return _service.DeleteRecord(formName, recordId);
        //    }
        //    return false;

        //}
        #endregion

    }
}


//switch (_formdata.FormName.ToLower())
//{
//    case "property":
//            // Let's create Property object from FormObject
//            Property _prop = new Property();

//            foreach (FormField _field in _formdata.FormFields)
//            {
//                _prop.SetPropertyValue(_field.FieldName, _field.FieldValue);
//            }

//            bSucceeded = _service.CreateUpdateRecord(EntityType.Property, _prop);
//            return Utility.ReturnRecordResponse(_modelState, bSucceeded);

//    case "contact":                       
//            // Let's create Contact object from FormObject
//            Contact _contact = new Contact();
//            foreach (FormField _field in _formdata.FormFields)
//            {
//                _contact.SetPropertyValue(_field.FieldName, _field.FieldValue);
//            }

//            bSucceeded = _service.CreateUpdateRecord( EntityType.Contact,_contact);
//            return Utility.ReturnRecordResponse(_modelState, bSucceeded);

//    default:
//        break;
//}