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

    //[RoutePrefix("api/Data")]
    // [Authorize]
    // [EnableCors(origins: "http://localhost")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        public dynamic GetAllClaims(Guid? userid, Guid? propertyid, string Type, string optionalSeachText = "")
        {
            return _service.GetAllClaims(userid, propertyid, Type, optionalSeachText);
        }


        [ActionName("GetUserProperty")]
        [HttpGet]
        public dynamic GetUserProperty(Guid userid)
        {
            return _service.GetUserProperty(userid);
        }

        [ActionName("GetUserPropertyForClaimPrint")]
        [HttpGet]
        public dynamic GetUserPropertyForClaimPrint(string userid)
        {
            return _service.GetUserPropertyForClaimPrint(userid);
        }


        #region View Claim

        [ActionName("GetClaimDetails")]
        [HttpGet]
        public dynamic GetClaimDetails(string Claim, char Type)
        {
            return _service.GetClaimDetails(Claim, Type);
        }


        [ActionName("GetClaimDetailsForPrint")]
        [HttpGet]
        public dynamic GetClaimDetailsForPrint(string Claim, char Type)
        {

            return _service.GetClaimDetailsForPrint(Claim, Type);
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
            fa.PositionDate = Convert.ToDateTime(HttpContext.Current.Request.Params["positiondate"]);
            fa.EmployeeName = HttpContext.Current.Request.Params["empname"].ToString();
            fa.Community = HttpContext.Current.Request.Params["community"].ToString();
            fa.ApartmentMarketRentalValue = Convert.ToDecimal(HttpContext.Current.Request.Params["marketvalue"]);
            fa.EmployeeMonthlyRent = Convert.ToDecimal(HttpContext.Current.Request.Params["emprent"]);
            fa.RentalPaymentResidencyAt = HttpContext.Current.Request.Params["residence"].ToString();
            fa.PropertyManager = "";
            fa.SignatureOfPropertyManager = HttpContext.Current.Request.Params["propertymanager"].ToString();
            fa.SignatureOfEmployee = HttpContext.Current.Request.Params["signature"].ToString();
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
            fa.kitordered = Convert.ToDateTime(HttpContext.Current.Request.Params["kitordered"]);
            fa.boardingcallscheduled = Convert.ToDateTime(HttpContext.Current.Request.Params["callscheduled"]);
            fa.Allocation = HttpContext.Current.Request.Params["allocation"].ToString();
            fa.esignature = HttpContext.Current.Request.Params["esignature"].ToString();
            fa.edate = Convert.ToDateTime(HttpContext.Current.Request.Params["edate"]);

            fa.msignature = HttpContext.Current.Request.Params["msignature"].ToString();
            fa.mdate = Convert.ToDateTime(HttpContext.Current.Request.Params["mdate"]);

            fa.rpmsignature = HttpContext.Current.Request.Params["rpmsignature"].ToString();
            fa.rpmdate = Convert.ToDateTime(HttpContext.Current.Request.Params["rpmdate"]);

          

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
            fa.TypeOfChange = HttpContext.Current.Request.Params["typeofchange"].ToString();
            fa.FromPropNum =Convert.ToDouble(HttpContext.Current.Request.Params["frompropnum"].ToString());
            fa.FromPropName = HttpContext.Current.Request.Params["frompropname"].ToString();
            fa.FromManager = HttpContext.Current.Request.Params["frommanager"].ToString();
            fa.ToPropNum = Convert.ToDouble(HttpContext.Current.Request.Params["topropnum"].ToString());
            fa.ToPropName = HttpContext.Current.Request.Params["topropname"].ToString();
            fa.ToManager = HttpContext.Current.Request.Params["tomanager"].ToString();

            fa.PayrollStatusChangeNoticeId = System.Guid.NewGuid();
            fa.PayChangeTo =Convert.ToDouble(HttpContext.Current.Request.Params["paychangeto"].ToString());
            fa.PayChangeFrom = Convert.ToDouble(HttpContext.Current.Request.Params["paychangefrom"].ToString());
            fa.PayChangeFullTime = Convert.ToBoolean(HttpContext.Current.Request.Params["chkfull"].ToString());
            fa.PayChangePartTime = Convert.ToBoolean(HttpContext.Current.Request.Params["chkpart"].ToString());
            fa.PayChangeHourly = Convert.ToBoolean(HttpContext.Current.Request.Params["chkhourly"].ToString());
            fa.PayChangeSalary = Convert.ToBoolean(HttpContext.Current.Request.Params["chksalary"].ToString());
            fa.BeginPayPeriod =Convert.ToDateTime(HttpContext.Current.Request.Params["beginpayperiod"].ToString());
            fa.La_Property1 = HttpContext.Current.Request.Params["prop1"].ToString();
            fa.La_Property1_Per = Convert.ToDouble(HttpContext.Current.Request.Params["prop1per"].ToString());
            fa.La_Property2 = HttpContext.Current.Request.Params["prop2"].ToString();
            fa.La_Property2_Per = Convert.ToDouble(HttpContext.Current.Request.Params["prop2per"].ToString());
            fa.Chk_CarAmount = Convert.ToBoolean(HttpContext.Current.Request.Params["chkcaramount"].ToString());
            fa.CarAmountPerMonth = Convert.ToDouble(HttpContext.Current.Request.Params["txtcaramount"].ToString());
            fa.Chk_PhoneAmount = Convert.ToBoolean(HttpContext.Current.Request.Params["chkphoneamount"].ToString());
            fa.PhoneAmountPerMonth = Convert.ToDouble(HttpContext.Current.Request.Params["txtphoneamount"].ToString());
          
                fa.FmlaYes = Convert.ToBoolean(HttpContext.Current.Request.Params["chkfmlayes"].ToString());
            fa.FmlaNo = Convert.ToBoolean(HttpContext.Current.Request.Params["chkfmlano"].ToString());
                fa.EnrolledBenefitsYes =Convert.ToBoolean(HttpContext.Current.Request.Params["chkbenefityes"].ToString());
            fa.EnrolledBenefitsYes = Convert.ToBoolean(HttpContext.Current.Request.Params["chkbenefitno"].ToString());
                fa.Leave_Purpose = HttpContext.Current.Request.Params["purpose"].ToString();
            fa.Leave_Begin = Convert.ToDateTime(HttpContext.Current.Request.Params["leavebegin"].ToString());
            fa.Leave_End = Convert.ToDateTime(HttpContext.Current.Request.Params["leaveend"].ToString());
           fa.Pto_Balance=Convert.ToDouble(HttpContext.Current.Request.Params["ptobalance"].ToString());
                fa.Notes1=HttpContext.Current.Request.Params["notes1"].ToString();
            fa.Notes2 = HttpContext.Current.Request.Params["notes2"].ToString();
            fa.ESignature = HttpContext.Current.Request.Params["esignature"].ToString();
            fa.EDate = Convert.ToDateTime(HttpContext.Current.Request.Params["edate"]);

            fa.MSignature = HttpContext.Current.Request.Params["msignature"].ToString();
            fa.MDate = Convert.ToDateTime(HttpContext.Current.Request.Params["mdate"]);
            fa.CreatedUser = new Guid(HttpContext.Current.Request.Params["CreatedBy"]);        
            fa.CreatedDateTime = DateTime.Now;

            return _service.InsertPayRollStatusChangeNotice(fa);
        }


        [ActionName("InsertRequisitionRequest")]
        [HttpPost]
        public dynamic InsertRequisitionRequest()
        {
            RequisitionRequest fa = new RequisitionRequest();
            fa.RequisitionRequestId = System.Guid.NewGuid();
            fa.PropertyName = HttpContext.Current.Request.Params["property"];
            fa.Date = Convert.ToDateTime(Convert.ToDateTime(HttpContext.Current.Request.Params["date"].ToString()));
            fa.RequestorName = HttpContext.Current.Request.Params["requestorname"].ToString();
            fa.RequestorPosition = HttpContext.Current.Request.Params["position"].ToString();
            fa.PositionCombined = HttpContext.Current.Request.Params["positioncombined"].ToString();
            fa.PositionOther = HttpContext.Current.Request.Params["txtother"].ToString();
            fa.ChkOtherPosition = Convert.ToBoolean(HttpContext.Current.Request.Params["chkposition"].ToString());
            fa.Type = HttpContext.Current.Request.Params["type"].ToString().Trim(',');
            fa.Post = HttpContext.Current.Request.Params["post"].ToString().Trim(',');
            fa.ChkNewPosition = Convert.ToBoolean(HttpContext.Current.Request.Params["chknewposition"].ToString());

            fa.ChkReplacementPosition = Convert.ToBoolean(HttpContext.Current.Request.Params["chkreplacementposition"].ToString());
            fa.ReplacingPerson = HttpContext.Current.Request.Params["replaceperson"].ToString();
            fa.ChkCarrollCareersIndeed = Convert.ToBoolean(HttpContext.Current.Request.Params["postindeed"].ToString());
            fa.ChkApartmentAssociation = Convert.ToBoolean(HttpContext.Current.Request.Params["postassociation"].ToString());
            fa.PostOther = HttpContext.Current.Request.Params["otherpost"].ToString();
            fa.ChkOtherPost = Convert.ToBoolean(HttpContext.Current.Request.Params["chkotherpost"].ToString());
            fa.SpecailInstructions =HttpContext.Current.Request.Params["specialinstructions"].ToString();
            fa.RequistionNumber = HttpContext.Current.Request.Params["requisitionnumber"].ToString();
            fa.DatePosted = Convert.ToDateTime(HttpContext.Current.Request.Params["dateposted"].ToString());
            fa.Notes = HttpContext.Current.Request.Params["notes"].ToString();
            
            fa.CreatedUser = new Guid(HttpContext.Current.Request.Params["CreatedBy"]);
            fa.CreatedDateTime = DateTime.Now;

            return _service.InsertRequisitionRequest(fa);
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
            fa.DischargedText = HttpContext.Current.Request.Params["discharge"].ToString();
            fa.QuitText = HttpContext.Current.Request.Params["quit"].ToString();
            fa.Reason = HttpContext.Current.Request.Params["reason"].ToString();
            fa.LackOfWork = HttpContext.Current.Request.Params["work"].ToString();
            fa.SSignature = HttpContext.Current.Request.Params["ssignature"].ToString();
            fa.SDate = Convert.ToDateTime(HttpContext.Current.Request.Params["sdate"]);
            fa.SMSignature = HttpContext.Current.Request.Params["smsignature"].ToString();
            fa.SMDate = Convert.ToDateTime(HttpContext.Current.Request.Params["smdate"]);
            fa.HRMSignature = HttpContext.Current.Request.Params["hrmsignature"].ToString();
            fa.HRMDate = Convert.ToDateTime(HttpContext.Current.Request.Params["hrmdate"]);

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

        [ActionName("InsertMileageLog")]
        [HttpPost]
        public dynamic InsertMileageLog(List<string> rows)
        {
            MileageLogHeader fa = new MileageLogHeader();
           
            fa.EmployeeName = HttpContext.Current.Request.Params["empname"].ToString();
            fa.OfficeAddress = HttpContext.Current.Request.Params["officeaddress"].ToString();
            // fa.PropertyName = HttpContext.Current.Request.Params["propertyname"].ToString();
            fa.ReportedMonthMileage = HttpContext.Current.Request.Params["mileagereported"].ToString();
            fa.TotalNumberOfMiles = Convert.ToDouble(HttpContext.Current.Request.Params["totalmiles"].ToString());
            fa.MonthlyMileageLogId = System.Guid.NewGuid();
            fa.TotalPrice = Convert.ToDouble(HttpContext.Current.Request.Params["totalprice"].ToString());
            fa.Signature =  HttpContext.Current.Request.Params["signature"].ToString();
            fa.ApprovedBy = HttpContext.Current.Request.Params["approvedby"].ToString();
            fa.CreatedUser = new Guid(HttpContext.Current.Request.Params["CreatedBy"]);
            fa.CreatedDatetime = DateTime.Now;

          var totalrows = HttpContext.Current.Request.Params["rows"].ToString();
            var allrows = totalrows.Split('|');

            List<MileageLogDetail> md = new List<MileageLogDetail>();

            foreach (var item in allrows)
            {
                var values = item.ToString().Split(',');
                var m = new MileageLogDetail();
                m.MileageLogDetailsId = System.Guid.NewGuid();
                m.Date = Convert.ToDateTime(values[0]);
                m.BillToProperty = values[1].ToString();
                m.Origin_Destination = values[2].ToString();
                m.Purpose = values[3].ToString();
                m.MileageLogId = fa.MonthlyMileageLogId;
                m.NumberOfMiles = Convert.ToDouble(values[4]);
                md.Add(m);
            }

            return _service.InsertMileageLog(fa,md);
        }

        [ActionName("GetMileageLog")]
        [HttpGet]
        public dynamic GetMileageLog(string riderid)
        {

            return _service.GetMileageLog(new Guid(riderid));
        }


        [ActionName("GetExpenseReimbursement")]
        [HttpGet]
        public dynamic GetExpenseReimbursement(string riderid)
        {

            return _service.GetExpenseReimbursement(new Guid(riderid));
        }

        [ActionName("GetRequisitionRequest")]
        [HttpGet]
        public dynamic GetRequisitionRequest(string riderid)
        {

            return _service.GetRequisitionRequest(new Guid(riderid));
        }

        [ActionName("GetAllHrForms")]
        [HttpGet]
        public dynamic GetAllHrForms(string FormType, string OptionalSeachText="")
        {
            return _service.GetAllHrForms(FormType, OptionalSeachText);
        }

        [ActionName("GetAllMileageForms")]
        [HttpGet]
        public dynamic GetAllMileageForms(string FormType, string userid)
        {
            return _service.GetAllMileageForms(FormType,userid, "");
        }



        [ActionName("GetHrFormCount")]
        [HttpGet]
        public dynamic GetHrFormCount()
        {
            return _service.GetHrFormCount();
        }

        #endregion


        #region Mileage Forms



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
            var _partners = _service.GetEquityPartners();
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
