using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Services.Models.Validation;
using System.Data;

namespace Carroll.Data.Services.Models
{
    public class DataService :IDataService
    {
        private IValidationDictionary _validationDictionary;
        private IDataRepository _repository;
       // private EntityUserRepository entityUserRepository;

        public DataService(IValidationDictionary validationDictionary) 
            : this(validationDictionary, new EntityDataRepository())
        {}

        public DataService(IValidationDictionary validationDictionary, IDataRepository repository)
        {
            _validationDictionary = validationDictionary;
            _repository = repository;
        }

        public dynamic GetAllClaims(Guid? userid, Guid? propertyid,string Type, int orderby,string optionalSeachText= "")
        {
            return _repository.GetAllClaims(userid,propertyid, Type, optionalSeachText, orderby);
        }

        public dynamic GetUserProperty(Guid userid)
        {
            return _repository.GetUserProperty(userid);
        }
        public   dynamic GetUserPropertyForClaimPrint(string userid)
        {
            return _repository.GetUserPropertyForClaimPrint(userid);
        }

        public string GetPropertyName(int PropertyNumber)
        {
            return _repository.GetPropertyName(PropertyNumber);
        }


        public  dynamic GetNewHireRejectionDetails(string Refid)
        {
            return _repository.GetNewHireRejectionDetails(Refid);
        }

        public dynamic GetPayRollRejectionDetails(string Refid)
        {
            return _repository.GetPayRollRejectionDetails(Refid);
        }

        public dynamic GetHrFormLogActivity(string FormType, string RecordId)
        {
            return _repository.GetHrFormLogActivity(FormType, RecordId);
        }

        public string GetPropertyNameManager(int PropertyNumber)
        {
            return _repository.GetPropertyNameManager(PropertyNumber);
        }
        public string GetPropertyNumberNameManager(string PropertyNumber)
        {
            return _repository.GetPropertyNumberNameManager(PropertyNumber);
        }

        #region [Records]
        public dynamic GetRecords(EntityType entityType, string optionalSeachText = "")
        {
            return  _repository.GetRecords(entityType,optionalSeachText);

        }
        public  dynamic GetEquityPartners()
        {
            return _repository.GetEquityPartners();

        }
      
        public  dynamic GetRecordsWithConfig(EntityType entityType, string optionalSeachText = "")
        {
            return _repository.GetRecordsWithConfig(entityType, optionalSeachText);
        }

        public dynamic GetRecord(EntityType entityType,string recordId)
        {
            return _repository.GetRecord(entityType,recordId);

        }

        public dynamic CreateUpdateRecord(EntityType entityType,dynamic obj)
        {
            //  if (!Carroll.Data.Services.Helpers.Utility.ValidateFormData(_validationDictionary, FormData)) return Guid.Empty;
            return _repository.CreateUpdateRecord(entityType,obj);

        }

        public bool DeleteRecord(EntityType entityType,string recordId)
        {
            return _repository.DeleteRecord(entityType,recordId);
        }

        public dynamic GetRuntimeClassInstance(string className)
        {
            return _repository.GetRuntimeClassInstance(className);
        }

        public dynamic GetClaimDetails(string Claim, char Type)
        {
            return _repository.GetClaimDetails(Claim,Type);
        }

        public dynamic GetClaimDetailsForPrint(string Claim, char Type)
        {
            return _repository.GetPrintClaim(Claim, Type);
        }

        public dynamic GetExportClaim(string Claim, char Type)
        {
            return _repository.GetExportClaim(Claim, Type);
        }

        public dynamic InsertComment(FormComment obj)
        {
            return _repository.InsertComment(obj);
        }

        public dynamic InsertEmployeeLeaseRider(EmployeeLeaseRaider obj)
        {
            return _repository.InsertEmployeeLeaseRider(obj);
        }

        public dynamic GetEmployeeLeaseRider(Guid obj)
        {
            return _repository.GetEmployeeLeaseRider(obj);
        }

        public dynamic InsertEmployeeNewHireNotice(EmployeeNewHireNotice obj, string Type)
        {
            return _repository.InsertEmployeeNewHireNotice(obj,Type);
        }

        public dynamic GetEmployeeNewHireNotice(Guid obj)
        {
            return _repository.GetEmployeeNewHireNotice(obj);
        }
      public  dynamic GetAllContactsHighRolesInclude(string search)
        {
            return _repository.GetAllContactsHighRolesInclude(search);
        }
        public  dynamic UpdateWorkflowEmployeeNewHireNotice(string Action, string RefId, string Sign, DateTime? edate)
        {
            return _repository.UpdateWorkflowEmployeeNewHireNotice(Action,RefId,Sign,edate,"","");
        }

      public  dynamic UpdateRequisitionRequest(Guid Refid, string RequisitionNumber, string notes, DateTime dateposted)
        {
            return _repository.UpdateRequisitionRequest(Refid, RequisitionNumber, notes, dateposted);
        }

        public dynamic InsertRequisitionRequest(RequisitionRequest obj)
        {
            return _repository.InsertRequistionRequest(obj);
        }

        public string GetPropertyManager(Guid PropertyId)
        {
            return _repository.GetPropertyManager(PropertyId);
        }

        public dynamic GetRequisitionRequest(Guid obj)
        {
            return _repository.GetRequisitionRequest(obj);
        }

        public dynamic InsertMileageLog(MileageLogHeader mh,List<MileageLogDetail> obj)
        {
            return _repository.InsertMileageLog(mh,obj);
        }

        public dynamic GetMileageLog(Guid obj)
        {
            return _repository.GetMileageLog(obj);
        }

        public dynamic InsertExpenseReimbursement(ExpenseReimbursementHeader eh,List<ExpenseReimbursementDetail> md)
        {
            return _repository.InsertExpenseReimbursement(eh,md);
        }

        public dynamic GetExpenseReimbursement(Guid obj)
        {
            return _repository.GetExpenseReimbursement(obj);
        }

        public dynamic InsertPayRollStatusChangeNotice(PayrollStatusChangeNotice obj)
        {
            return _repository.InsertPayRollStatusChangeNotice(obj);
        }

        public dynamic GetPayRollStatusChangeNotice(Guid obj)
        {
            return _repository.GetPayRollStatusChangeNotice(obj);
        }
               
        public List<proc_getcontactsforexcel_Result1> GetAllContactsForExcel()
        {           
                return _repository.GetAllContactsForExcel().ToList();           
        }

        public List<proc_getequitypartnersforexcel_Result1> GetAllEquityPartnersForExcel()
        {         
                return _repository.GetAllEquityPartnersForExcel().ToList();   
        }

        public List<proc_getpropertiesforexcelupdate_Result> GetAllPropertiesForExcel()
        {          
                return _repository.GetAllPropertiesForExcel().ToList();           
        }


        public dynamic ImportContactTableFromExcel(DataTable dt)
        {
            return _repository.ImportContactTableFromExcel(dt);
        }

        public dynamic ImportEquityPartnerTableFromExcel(DataTable dt)
        {
            return _repository.ImportEquityPartnerTableFromExcel(dt);
        }

        public dynamic ImportPropertiesTableFromExcel(DataTable dt)
        {
            return _repository.ImportPropertiesTableFromExcel(dt);
        }
        public dynamic InsertNoticeOfEmployeeSeperation(NoticeOfEmployeeSeperation obj)
        {
            return _repository.InsertNoticeOfEmployeeSeperation(obj);
        }

        public dynamic GetNoticeOfEmployeeSeperation(Guid obj)
        {
            return _repository.GetNoticeOfEmployeeSeperation(obj);
        }
        public dynamic GetUserClaimCount(Guid userid)
        {
            return _repository.GetUserClaimCount(userid);
        }

        public dynamic InsertAttachment(List<FormAttachment> formAttachment)
        {
            return _repository.InsertAttachment(formAttachment);
        }

      public  dynamic GetAllHrForms(Guid? userid, string FormType, string OptionalSeachText)
        {
            return _repository.GetAllHrForms(userid,FormType, OptionalSeachText);
        }

        public dynamic GetAllMileageForms(string FormType,string userid, string OptionalSeachText)
        {
            return _repository.GetAllMileageForms(FormType, new Guid(userid), OptionalSeachText);
        }

        public dynamic InsertResidentReferralRequest(ResidentReferalSheet eh)
        {
            return _repository.InsertResidentReferralRequest(eh);
        }

        public dynamic GetResidentReferralRequest(Guid obj)
        {
            return _repository.GetResidentReferralRequest(obj);
        }
        
        public dynamic InsertResidentReferralContact(ResidentContactInformation mlh, List<ResidentContactInformation_Residents> rrs, List<ResidentContactInformation_OtherOccupants> ors, List<ResidentContactInformation_Vehicles> vhs)
        {
            return _repository.InsertResidentReferralContact(mlh,rrs,ors,vhs);
        }

        public PrintResidentContact GetResidentReferralContact(Guid riderid)
        {
            return _repository.GetResidentReferralContact(riderid);
        }

        public  dynamic UpdateNewHireRejectionStatus(string status, string reason, string refid, string refuser)
        {
            return _repository.UpdateNewHireRejectionStatus(status, reason, refid, refuser);
        }


        public dynamic UpdatePayRollRejectionStatus(string status, string reason, string refid, string refuser)
        {
            return _repository.UpdatePayRollRejectionStatus(status, reason, refid, refuser);
        }


        public dynamic GetHrFormCount()
        {
            return _repository.GetHrFormCount();
        }

        #endregion

        public List<CarrollPosition> GetAllCarrollPositions()
        {
            return _repository.GetAllCarrollPositions();
        }
        public List<CarrollPosition> GetAllCarrollPositionsByType(string Type)
        {
            return _repository.GetAllCarrollPositionsByType(Type);
        }

        public List<CarrollPayPeriod> GetAllCarrollPayPerilds( )
        {
            return _repository.GetAllCarrollPayPerilds();
        }

     public   dynamic GetDynamicLinkStatus(Guid refid)
        {
            return _repository.GetDynamicLinkStatus(refid);
        }

        public void ErrorLog(ErrorLog errorLog)
        {
             _repository.ErrorLog(errorLog);
        }

    }
}
