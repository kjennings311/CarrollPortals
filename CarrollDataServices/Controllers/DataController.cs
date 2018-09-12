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
using System.Web;
//using System.Web.Mvc;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
//using MongoDB.Driver.Builders;
using Carroll.Data.Services.Models.MongoModels;

namespace Carroll.Data.Services.Controllers
{
    //  [EnableCors(origins = new[] { "http://localhost", "http://sample.com" })]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[RoutePrefix("api/Data")]
    // [Authorize]
    // [EnableCors(origins: "http://localhost")]

    public class DataController : ApiController
    {
        private IDataService _service;
        private IValidationDictionary _modelState;

    //    MongoClient _client;
        IMongoDatabase _db;

        public DataController()
        {
            _modelState = new ModelStateWrapper(this.ModelState);
            _service = new DataService(_modelState, new EntityDataRepository());
            //_client = new MongoClient("mongodb://localhost:27017");
            //_db = _client.GetDatabase("DynamicForms");

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
        public dynamic GetRecords(EntityType entityType, string optionalText = "")
        {

            return _service.GetRecords(entityType, optionalText);

        }


        [ActionName("Index")]
        [HttpGet]
        public string Index()
        {
            return _db.GetCollection<Models.MongoModels.Form>("Forms").ToJson();
        }

       


        [ActionName("GetRecordsWithConfig")]
        [HttpGet]
        public dynamic GetRecordsWithConfig(EntityType entityType, string optionalText = "")
        {
            return _service.GetRecordsWithConfig(entityType, optionalText);

        }

        [ActionName("GetAllClaims")]
        [HttpGet]
        public dynamic GetAllClaims(Guid? userid, Guid? propertyid, string optionalSeachText = "")
        {
            return _service.GetAllClaims(userid, propertyid, optionalSeachText);
        }

        #region View Claim

        [ActionName("GetClaimDetails")]
        [HttpGet]
        public dynamic GetClaimDetails(string Claim, char Type)
        {

            return _service.GetClaimDetails(Claim, Type);
        }

        [ActionName("InsertComment")]
        [HttpPost]      
        public dynamic InsertComment([FromBody] FormComment obj)
        {           
            return _service.InsertComment(obj);           
        }

        [ActionName("InsertAttachment")]
        [HttpPost]
        public dynamic InsertAttachment()
        {

            FormAttachment fa = new FormAttachment();

            var randomstring= DateTime.Now.ToString("yyMMddHHmmssff");
            var filename = "";

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files["file"];

                if (httpPostedFile != null)
                {
                    // Validate the uploaded image(optional)

                    // Get the complete file path
                    filename = httpPostedFile.FileName;
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), randomstring+httpPostedFile.FileName);

                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);
                }
            }
            fa.RefFormType=Convert.ToInt16(HttpContext.Current.Request.Params["RefFormType"]);
            fa.RefId = new Guid(HttpContext.Current.Request.Params["RefId"]);
            fa.UploadedBy = new Guid(HttpContext.Current.Request.Params["UploadedBy"]);
            fa.UploadedByName = HttpContext.Current.Request.Params["UploadedByName"];
            fa.At_Name =filename;
            fa.At_FileName = randomstring+filename;
            return _service.InsertAttachment(fa);            
        }


        public dynamic GetUserClaimCount(Guid userid)
        {
            return _service.GetUserClaimCount(userid);
        }


        #endregion


        
        [ActionName("DeleteRecord")]
        [HttpPost]
        //[ValidateInput(false)]
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

            var _users = _service.GetRecords(EntityType.Contact,q.ToLower());
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

        #region  HR Forms
        [ActionName("InsertEmployeeLeaseRider")]
        [HttpPost]
        public dynamic InsertEmployeeLeaseRider()
        {

            EmployeeLeaseRaider fa = new EmployeeLeaseRaider();
            fa.Date = Convert.ToDateTime(HttpContext.Current.Request.Params["date"]);
            fa.EmployeeName = HttpContext.Current.Request.Params["empname"].ToString();
            fa.Community = HttpContext.Current.Request.Params["community"].ToString();
            fa.ApartmentMarketRentalValue = Convert.ToDecimal(HttpContext.Current.Request.Params["marketvalue"]);
            fa.EmployeeMonthlyRent = Convert.ToDecimal(HttpContext.Current.Request.Params["emprent"]);
            fa.RentalPaymentResidencyAt = HttpContext.Current.Request.Params["residence"].ToString();
            fa.PropertyManager = "";
            fa.EmployeeLeaseRiderId= System.Guid.NewGuid();
            fa.Position = HttpContext.Current.Request.Params["position"].ToString();
            fa.CreatedUser = new Guid( HttpContext.Current.Request.Params["CreatedBy"]);
            fa.CreatedDatetime = DateTime.Now;

            return _service.InsertEmployeeLeaseRider(fa);
        }

        [ActionName("GetEmployeeLeaseRider")]
        [HttpGet]
        public dynamic GetEmployeeLeaseRider(string riderid)
        {

            return _service.GetEmployeeLeaseRider(new Guid(riderid));
        }


        [ActionName("InsertEmployeeNewHireNotice")]
        [HttpPost]
        public dynamic InsertEmployeeNewHireNotice()
        {
            EmployeeNewHireNotice fa = new EmployeeNewHireNotice();
            fa.StartDate =Convert.ToDateTime(Convert.ToDateTime(HttpContext.Current.Request.Params["startdate"]));
            fa.EmployeeName = HttpContext.Current.Request.Params["empname"].ToString();
            fa.EmployeeSocialSecuirtyNumber = HttpContext.Current.Request.Params["securitynumber"].ToString();
            fa.EmailAddress =HttpContext.Current.Request.Params["email"].ToString();
            fa.Manager = HttpContext.Current.Request.Params["manager"].ToString();
            fa.Location = HttpContext.Current.Request.Params["location"].ToString();           
            fa.EmployeeHireNoticeId = System.Guid.NewGuid();
            fa.Position = HttpContext.Current.Request.Params["position"].ToString();
            fa.Position_Exempt = HttpContext.Current.Request.Params["exempt"].ToString();
            fa.Position_NonExempt = HttpContext.Current.Request.Params["nonexempt"].ToString();
            fa.Status = HttpContext.Current.Request.Params["status"].ToString();
            fa.Wage_Salary = HttpContext.Current.Request.Params["salary"].ToString();
            fa.Allocation = HttpContext.Current.Request.Params["allocation"].ToString();         
            fa.CreatedUser = new Guid(HttpContext.Current.Request.Params["CreatedBy"]);
            fa.CreatedDateTime = DateTime.Now;

            return _service.InsertEmployeeNewHireNotice(fa);
        }

        [ActionName("GetEmployeeNewHireNotice")]
        [HttpGet]
        public dynamic GetEmployeeNewHireNotice(string riderid)
        {
            return _service.GetEmployeeNewHireNotice(new Guid(riderid));
        }


        [ActionName("InsertPayRollStatusChangeNotice")]
        [HttpPost]
        public dynamic InsertPayRollStatusChangeNotice()
        {
            PayrollStatusChangeNotice fa = new PayrollStatusChangeNotice();
            fa.ChangeEffectiveDate = Convert.ToDateTime(Convert.ToDateTime(HttpContext.Current.Request.Params["effectivedate"]));
            fa.EmployeeName = HttpContext.Current.Request.Params["empname"].ToString();
            fa.Manager = HttpContext.Current.Request.Params["manager"].ToString();
            fa.TodayDate =Convert.ToDateTime(HttpContext.Current.Request.Params["todaydate"].ToString());
            fa.Wage_Salary = HttpContext.Current.Request.Params["salary"].ToString();
            fa.Client_Location = HttpContext.Current.Request.Params["location"].ToString();
            fa.PayrollStatusChangeNoticeId = System.Guid.NewGuid();
            fa.Position = HttpContext.Current.Request.Params["position"].ToString();
            fa.NewHire = Convert.ToBoolean(HttpContext.Current.Request.Params["newhire"].ToString());
            fa.ReHire = Convert.ToBoolean(HttpContext.Current.Request.Params["rehire"].ToString());
            fa.Transfer = Convert.ToBoolean(HttpContext.Current.Request.Params["transfer"].ToString());
            fa.SsHash = HttpContext.Current.Request.Params["ss"].ToString();
            fa.DOB = Convert.ToDateTime(HttpContext.Current.Request.Params["dob"].ToString());
            fa.Exempt = HttpContext.Current.Request.Params["exempt"].ToString();
            fa.NonExempt = HttpContext.Current.Request.Params["nonexempt"].ToString();
            fa.StreetAddress = HttpContext.Current.Request.Params["streetaddress"].ToString();
            fa.City_State_Zip = HttpContext.Current.Request.Params["city_state_zip"].ToString();
            fa.Phone = HttpContext.Current.Request.Params["phone"].ToString();
            fa.Status_FullTime_PartTime = HttpContext.Current.Request.Params["status"].ToString();
            if(!string.IsNullOrEmpty(HttpContext.Current.Request.Params["payratefrom"].ToString()))
            fa.Change_Pay_Rate_From = Convert.ToDouble(HttpContext.Current.Request.Params["payratefrom"].ToString());
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["payrateto"].ToString()))
                fa.Change_Pay_Rate_To = Convert.ToDouble(HttpContext.Current.Request.Params["payrateto"].ToString());
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["propertyfrom"].ToString()))
                fa.Change_Property_From = HttpContext.Current.Request.Params["propertyfrom"].ToString();
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["propertyto"].ToString()))
                fa.Change_Property_To = HttpContext.Current.Request.Params["propertyto"].ToString();
            fa.Address_ContactInfo = HttpContext.Current.Request.Params["address_contactinfo"].ToString();
            fa.Date_Of_Suspence = HttpContext.Current.Request.Params["dateofsuspence"].ToString();
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["suspence_paid"].ToString()))
                fa.Suspence_Paid = Convert.ToDouble(HttpContext.Current.Request.Params["suspence_paid"].ToString());
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["suspence_unpaid"].ToString()))
                fa.Suspence_UnPaid = Convert.ToDouble(HttpContext.Current.Request.Params["suspence_unpaid"].ToString());
            fa.Leave_Absence = HttpContext.Current.Request.Params["leaveabsence"].ToString();
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["leave_paid"].ToString()))
                fa.Leave_Paid = Convert.ToDouble(HttpContext.Current.Request.Params["leave_paid"].ToString());
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["leave_unpaid"].ToString()))
                fa.Leave_UnPaid = Convert.ToDouble(HttpContext.Current.Request.Params["leave_unpaid"].ToString());
            fa.Explanation = HttpContext.Current.Request.Params["explanation"].ToString();
            fa.Allocation = HttpContext.Current.Request.Params["allocation"].ToString();
            fa.CreatedUser = new Guid(HttpContext.Current.Request.Params["CreatedBy"]);
            fa.CreatedDateTime = DateTime.Now;

            return _service.InsertPayRollStatusChangeNotice(fa);
        }

        [ActionName("GetPayRollStatusChangeNotice")]
        [HttpGet]
        public dynamic GetPayRollStatusChangeNotice(string riderid)
        {

            return _service.GetPayRollStatusChangeNotice(new Guid(riderid));
        }

        [ActionName("InsertNoticeOfEmployeeSeperation")]
        [HttpPost]
        public dynamic InsertNoticeOfEmployeeSeperation()
        {
            NoticeOfEmployeeSeperation fa = new NoticeOfEmployeeSeperation();
            fa.EffectiveDateOfChange = Convert.ToDateTime(Convert.ToDateTime(HttpContext.Current.Request.Params["datechange"]));
            fa.EmployeeName = HttpContext.Current.Request.Params["empname"].ToString();
            fa.EligibleForReHire = Convert.ToBoolean(HttpContext.Current.Request.Params["rehire"].ToString());
            fa.PropertyName = HttpContext.Current.Request.Params["propertyname"].ToString();
            fa.PropertyNumber = HttpContext.Current.Request.Params["propertynumber"].ToString();
            fa.JobTitle = HttpContext.Current.Request.Params["jobtitile"].ToString();
            fa.EmployeeSeperationId = System.Guid.NewGuid();
            fa.Policty_Voilated = HttpContext.Current.Request.Params["policty"].ToString();
            fa.AdditionalRemarks = HttpContext.Current.Request.Params["remarks"].ToString();
            fa.DocumentationAvailable = HttpContext.Current.Request.Params["documentaiton"].ToString();
            fa.WarningGiven_Dates = HttpContext.Current.Request.Params["warningdates"].ToString();
            fa.EquipmentKeysReturned =Convert.ToBoolean( HttpContext.Current.Request.Params["keysreturned"].ToString());
            fa.C2WeeeksNoticeGiven= Convert.ToBoolean(HttpContext.Current.Request.Params["noticegives"].ToString());
            fa.VacationPaidOut =Convert.ToBoolean( HttpContext.Current.Request.Params["vacationpaidout"].ToString());
            if(!string.IsNullOrEmpty(HttpContext.Current.Request.Params["balance"].ToString()))
            fa.VacationBalance = Convert.ToDouble(HttpContext.Current.Request.Params["balance"].ToString());
            fa.Notes_Comments = HttpContext.Current.Request.Params["comments"].ToString();
            fa.CreatedUser = new Guid(HttpContext.Current.Request.Params["CreatedBy"]);
            fa.CreatedDateTime = DateTime.Now;
            return _service.InsertNoticeOfEmployeeSeperation(fa);
        }

        [ActionName("GetNoticeOfEmployeeSeperation")]
        [HttpGet]
        public dynamic GetNoticeOfEmployeeSeperation(string riderid)
        {

            return _service.GetNoticeOfEmployeeSeperation(new Guid(riderid));
        }

        [ActionName("GetAllHrForms")]
        [HttpGet]
        public dynamic GetAllHrForms(string FormType, string OptionalSeachText="")
        {
            return _service.GetAllHrForms(FormType, OptionalSeachText);
        }

        [ActionName("GetHrFormCount")]
        [HttpGet]
        public dynamic GetHrFormCount()
        {
            return _service.GetHrFormCount();
        }

        #endregion




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
