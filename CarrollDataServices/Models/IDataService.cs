using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;

namespace Carroll.Data.Services.Models
{
    public interface IDataService
    {

        dynamic GetRecords(EntityType entityType, string optionalSeachText = "");

        dynamic GetRecordsWithConfig(EntityType entityType, string optionalSeachText = "");

        dynamic GetRecord(EntityType entityType, string recordId);
        dynamic GetAllClaims(Guid? userid, Guid? propertyid, string Type, string optionalSeachText);

        dynamic CreateUpdateRecord(EntityType entityType,dynamic obj);

        bool DeleteRecord(EntityType entityType,string recordId);
       
        dynamic GetRuntimeClassInstance(string className);
        dynamic GetClaimDetails(string Claim, char Type);
        dynamic GetClaimDetailsForPrint(string Claim, char Type);
        dynamic GetExportClaim(string Claim, char Type);
        dynamic GetUserPropertyForClaimPrint(string userid);
        dynamic InsertComment(FormComment obj);
        dynamic InsertAttachment(FormAttachment formAttachment);
        dynamic InsertEmployeeLeaseRider(EmployeeLeaseRaider Elr);
        dynamic GetEmployeeLeaseRider(Guid Elr);
        dynamic InsertEmployeeNewHireNotice(EmployeeNewHireNotice Elr,string Type);
        dynamic GetEmployeeNewHireNotice(Guid Elr);
        dynamic GetUserClaimCount(Guid userid);
        dynamic GetEquityPartners();
        dynamic GetUserProperty(Guid userid);
        dynamic InsertPayRollStatusChangeNotice(PayrollStatusChangeNotice formAttachment);
        dynamic GetPayRollStatusChangeNotice(Guid riderid);
        dynamic InsertRequisitionRequest(RequisitionRequest requisitionRequest);
        dynamic GetRequisitionRequest(Guid riderid);

        dynamic InsertNoticeOfEmployeeSeperation(NoticeOfEmployeeSeperation formAttachment);
        dynamic GetNoticeOfEmployeeSeperation(Guid riderid);
        dynamic GetAllHrForms(Guid? userid, string FormType, string OptionalSeachText);
        dynamic GetAllMileageForms(string FormType, string userid, string OptionalSeachText);
        dynamic InsertMileageLog(MileageLogHeader mlh,List<MileageLogDetail> mld);
        dynamic GetMileageLog(Guid riderid);
        dynamic InsertExpenseReimbursement(ExpenseReimbursementHeader mlh, List<ExpenseReimbursementDetail> mld);
        dynamic GetExpenseReimbursement(Guid riderid);
        dynamic InsertResidentReferralRequest(ResidentReferalSheet mlh);
        dynamic GetResidentReferralRequest(Guid riderid);
        dynamic InsertResidentReferralContact(ResidentContactInformation mlh, List<ResidentContactInformation_Residents> rrs, List<ResidentContactInformation_OtherOccupants> ors, List<ResidentContactInformation_Vehicles> vhs);
        dynamic UpdateWorkflowEmployeeNewHireNotice(string Action, string RefId, string Sign, DateTime? edate);
        PrintResidentContact GetResidentReferralContact(Guid riderid);
        dynamic UpdateRequisitionRequest(Guid Refid, string RequisitionNumber, string notes, DateTime dateposted);
        dynamic GetHrFormCount();

        List<CarrollPosition> GetAllCarrollPositions();
        List<CarrollPayPeriod> GetAllCarrollPayPerilds();

        List<proc_getcontactsforexcel_Result1> GetAllContactsForExcel();
        List<proc_getequitypartnersforexcel_Result1> GetAllEquityPartnersForExcel();
        List<proc_getpropertiesforexcelupdate_Result> GetAllPropertiesForExcel();
        dynamic UpdateNewHireRejectionStatus(string status, string reason, string refid, string refuser);
        dynamic ImportContactTableFromExcel(DataTable dt);
        dynamic ImportEquityPartnerTableFromExcel(DataTable dt);
        dynamic ImportPropertiesTableFromExcel(DataTable dt);
        string GetPropertyName(int PropertyNumber);
        string GetPropertyNameManager(int PropertyNumber);
        string GetPropertyManager(Guid PropertyId);
        string GetPropertyNumberNameManager(string PropertyNumber);
        dynamic GetHrFormLogActivity(string FormType, string RecordId);
        dynamic GetNewHireRejectionDetails(string Refid);
        void ErrorLog(ErrorLog errorLog);
        dynamic GetDynamicLinkStatus(Guid refid);
        dynamic GetAllContactsHighRolesInclude(string search);

       // dynamic SendClaimUpdatesLastWeek();
    }
}
