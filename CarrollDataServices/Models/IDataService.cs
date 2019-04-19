using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
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
        dynamic GetUserPropertyForClaimPrint(string userid);
        dynamic InsertComment(FormComment obj);
        dynamic InsertAttachment(FormAttachment formAttachment);
        dynamic InsertEmployeeLeaseRider(EmployeeLeaseRaider Elr);
        dynamic GetEmployeeLeaseRider(Guid Elr);
        dynamic InsertEmployeeNewHireNotice(EmployeeNewHireNotice Elr);
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
        dynamic GetAllHrForms(string FormType, string OptionalSeachText);
        dynamic GetAllMileageForms(string FormType, string userid, string OptionalSeachText);
        dynamic InsertMileageLog(MileageLogHeader mlh,List<MileageLogDetail> mld);
        dynamic GetMileageLog(Guid riderid);
        dynamic InsertExpenseReimbursement(ExpenseReimbursementHeader mlh, List<ExpenseReimbursementDetail> mld);
        dynamic GetExpenseReimbursement(Guid riderid);
        
        dynamic GetHrFormCount();
        List<CarrollPosition> GetAllCarrollPositions();
        List<CarrollPayPeriod> GetAllCarrollPayPerilds();
        string GetPropertyName(int PropertyNumber);
        string GetPropertyNameManager(int PropertyNumber);
    }
}
