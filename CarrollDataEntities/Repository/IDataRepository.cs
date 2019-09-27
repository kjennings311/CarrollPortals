
    using System;
    using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Carroll.Data.Entities.Repository
{
    /// <summary>
    /// Summary description for IAdminRepository
    /// </summary>
    public interface IDataRepository
    {
        dynamic CreateUpdateRecord(EntityType entityType,dynamic obj);

        dynamic GetRecord(EntityType entityType,string recordId);

        bool DeleteRecord(EntityType entityType,string recordId);
        dynamic GetEquityPartners();
        dynamic GetRecords(EntityType entityType, string optionalSeachText = "");
        dynamic GetRecordsWithConfig(EntityType entityType, string optionalSeachText = "");
        dynamic GetAllClaims(Guid? userid,Guid? propertyid,string Type, string OptionalSeachText);
        dynamic GetAllHrForms(string FormType, string OptionalSeachText);
        dynamic GetHrFormCount();
        dynamic GetUserClaimCount(Guid userid);
        dynamic GetClaimDetails(string Claim, char Type);
        dynamic GetPrintClaim(string Claim, char Type);
        dynamic GetExportClaim(string Claim, char Type);
        dynamic GetUserPropertyForClaimPrint(string userid);
        dynamic InsertComment(FormComment obj);
        dynamic InsertAttachment(FormAttachment formAttachment);
        dynamic InsertEmployeeLeaseRider(EmployeeLeaseRaider formAttachment);
        dynamic GetEmployeeLeaseRider(Guid riderid);
        dynamic InsertEmployeeNewHireNotice(EmployeeNewHireNotice formAttachment,string Type);
        dynamic GetEmployeeNewHireNotice(Guid riderid);

        dynamic InsertPayRollStatusChangeNotice(PayrollStatusChangeNotice formAttachment);
        dynamic GetPayRollStatusChangeNotice(Guid riderid);
        dynamic InsertRequistionRequest(RequisitionRequest requisitionRequest);
        dynamic GetRequisitionRequest(Guid riderid);
        dynamic InsertNoticeOfEmployeeSeperation(NoticeOfEmployeeSeperation formAttachment);
        dynamic GetNoticeOfEmployeeSeperation(Guid riderid);
        dynamic GetUserProperty(Guid userid);
        dynamic InsertMileageLog(MileageLogHeader mlh, List<MileageLogDetail> mld);
        dynamic GetMileageLog(Guid riderid);
        dynamic InsertExpenseReimbursement(ExpenseReimbursementHeader mlh, List<ExpenseReimbursementDetail> mld);
        dynamic GetExpenseReimbursement(Guid riderid);

        dynamic InsertResidentReferralRequest(ResidentReferalSheet mlh);
        dynamic GetResidentReferralRequest(Guid riderid);
        dynamic UpdateWorkflowEmployeeNewHireNotice(string Action, string RefId, string Sign, DateTime? edate,string browser, string ipaddress);

        dynamic UpdateWorkflowEmployeeLeaseRider(string Action, string RefId, string Sign, DateTime? edate,string browser,string ipaddress);
        dynamic UpdateWorkflowPayRollStatusChangeNotice(string Action, string RefId, string Sign, DateTime? edate, string browser, string ipaddress);

        dynamic InsertResidentReferralContact(ResidentContactInformation mlh,List<ResidentContactInformation_Residents> rrs,List<ResidentContactInformation_OtherOccupants> ors,List<ResidentContactInformation_Vehicles> vhs);
        dynamic UpdateNewHireRejectionStatus(string status, string reason, string refid, string refuser);

        dynamic GetNewHireRejectionDetails(string Refid);
        PrintResidentContact GetResidentReferralContact(Guid riderid);

        void LogActivity(string ActivityDesc, string UserName, string UserGuid, string RecordId, string ActivityStatus);
        void HrLogActivity(string FormType, string RecordId, string ActivitySubject, string ActivityDesc,string UserGuid);
        void ErrorLog(ErrorLog errorLog);
        dynamic GetHrFormLogActivity(string FormType, string RecordId);
        dynamic GetAllMileageForms(string FormType, Guid userid, string optionalSeachText);
        dynamic UpdateRequisitionRequest(Guid Refid, string RequisitionNumber, string notes, DateTime dateposted);
        string GetPropertyManager(Guid PropertyId);
        List<CarrollPosition> GetAllCarrollPositions();
        List<CarrollPayPeriod> GetAllCarrollPayPerilds();


        List<proc_getcontactsforexcel_Result1> GetAllContactsForExcel();
        List<proc_getequitypartnersforexcel_Result1> GetAllEquityPartnersForExcel();
        List<proc_getpropertiesforexcelupdate_Result> GetAllPropertiesForExcel();

        dynamic ImportContactTableFromExcel(DataTable dt);
        dynamic ImportEquityPartnerTableFromExcel(DataTable dt);
       dynamic ImportPropertiesTableFromExcel(DataTable dt);


        //List<spProperties_Result> GetProperties(string optionalSeachText = "");
        dynamic GetRuntimeClassInstance(string className);

        string GetPropertyName(int PropertyNumber);
        string GetPropertyNameManager(int PropertyNumber);
        string GetPropertyNumberNameManager(string PropertyNumber);
        dynamic GetDynamicLinkStatus(Guid refid);
        dynamic GetAllContactsHighRolesInclude(string search);
    }
}