
    using System;
    using System.Collections.Generic;
    using System.Linq;

namespace Carroll.Data.Entities.Repository
{
    /// <summary>
    /// Summary description for IAdminRepository
    /// </summary>
    public interface IDataRepository
    {
        bool CreateUpdateRecord(EntityType entityType,dynamic obj);

        dynamic GetRecord(EntityType entityType,string recordId);

        bool DeleteRecord(EntityType entityType,string recordId);

        dynamic GetRecords(EntityType entityType, string optionalSeachText = "");
        dynamic GetRecordsWithConfig(EntityType entityType, string optionalSeachText = "");

        //List<spProperties_Result> GetProperties(string optionalSeachText = "");
        dynamic GetRuntimeClassInstance(string className);
    }
}