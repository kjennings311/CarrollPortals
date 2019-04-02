
    using System;
    using System.Collections.Generic;
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
        dynamic GetUserPropertyForClaimPrint(string userid);
        dynamic InsertComment(FormComment obj);
        dynamic InsertAttachment(FormAttachment formAttachment);
        dynamic InsertEmployeeLeaseRider(EmployeeLeaseRaider formAttachment);
        dynamic GetEmployeeLeaseRider(Guid riderid);
        dynamic InsertEmployeeNewHireNotice(EmployeeNewHireNotice formAttachment);
        dynamic GetEmployeeNewHireNotice(Guid riderid);

        dynamic InsertPayRollStatusChangeNotice(PayrollStatusChangeNotice formAttachment);
        dynamic GetPayRollStatusChangeNotice(Guid riderid);
        dynamic InsertRequistionRequest(RequisitionRequest requisitionRequest);
        dynamic GetRequisitionRequest(Guid riderid);
        dynamic InsertNoticeOfEmployeeSeperation(NoticeOfEmployeeSeperation formAttachment);
        dynamic GetNoticeOfEmployeeSeperation(Guid riderid);
        dynamic GetUserProperty(Guid userid);
        void LogActivity(string ActivityDesc, string UserName, string UserGuid, string RecordId, string ActivityStatus);
         dynamic GetAllMileageForms(string FormType, Guid userid, string optionalSeachText);

        //List<spProperties_Result> GetProperties(string optionalSeachText = "");
        dynamic GetRuntimeClassInstance(string className);
    }
}