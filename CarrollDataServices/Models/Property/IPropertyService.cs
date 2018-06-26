using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
namespace Carroll.Data.Services.Models.Properties
{
    public interface IPropertyService
    {
        bool CreateUpdateProperty(Property Property);
        List<spProperties_Result> GetProperties(string optionalSeachText = "");
        bool DeleteProperty(string PropertyId);
        Property GetProperty(string PropertyId);

    }
}
