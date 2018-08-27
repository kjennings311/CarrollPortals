
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

        dynamic GetRecords(EntityType entityType, string optionalSeachText = "");
        dynamic GetRecordsWithConfig(EntityType entityType, string optionalSeachText = "");
        dynamic GetAllClaims(Guid? userid,Guid? propertyid,string OptionalSeachText);
        dynamic GetUserClaimCount(Guid userid);
        dynamic GetClaimDetails(string Claim, char Type);
        dynamic InsertComment(FormComment obj);
        dynamic InsertAttachment(FormAttachment formAttachment);
        dynamic InsertEmployeeLeaseRider(EmployeeLeaseRaider formAttachment);
        dynamic GetEmployeeLeaseRider(Guid riderid);
        dynamic InsertEmployeeNewHireNotice(EmployeeNewHireNotice formAttachment);
        dynamic GetEmployeeNewHireNotice(Guid riderid);
        void LogActivity(string ActivityDesc, string UserName, string UserGuid, string RecordId, string ActivityStatus);

        //List<spProperties_Result> GetProperties(string optionalSeachText = "");
        dynamic GetRuntimeClassInstance(string className);
    }
}