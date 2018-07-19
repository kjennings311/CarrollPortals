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
        dynamic GetAllClaims(Guid? userid, Guid? propertyid, string optionalSeachText);

        bool CreateUpdateRecord(EntityType entityType,dynamic obj);

        bool DeleteRecord(EntityType entityType,string recordId);

        dynamic GetRuntimeClassInstance(string className);
        dynamic GetClaimDetails(string Claim, char Type);
        dynamic InsertComment(Guid Claim, dynamic obj);
        dynamic InsertAttachment(Guid Claim, dynamic obj);
    }
}
