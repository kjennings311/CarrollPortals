
    using System;
    using System.Collections.Generic;
    using System.Linq;

namespace Carroll.Data.Entities.Repository
{
    /// <summary>
    /// Summary description for IAdminRepository
    /// </summary>
    public interface IPropertyRepository
    {
        List<spProperties_Result> GetProperties(string optionalSeachText = "");

        bool CreateUpdateProperty(Property Property);

        bool DeleteProperty(string PropertyId);

        Property GetProperty(string PropertyId);
    }
}