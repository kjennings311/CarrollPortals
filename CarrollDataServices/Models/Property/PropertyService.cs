using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Services.Models.Validation;
using Carroll.Data.Services.Models.Properties;

namespace Carroll.Data.Services.Models.Properties
{
    public class PropertyService :IPropertyService
    {
        private IValidationDictionary _validationDictionary;
        private IPropertyRepository _repository;
    

        public PropertyService(IValidationDictionary validationDictionary) 
            : this(validationDictionary, new EntityPropertyRepository())
        {}

        public PropertyService(IValidationDictionary validationDictionary, IPropertyRepository repository)
        {
            _validationDictionary = validationDictionary;
            _repository = repository;
        }

      

        public bool CreateUpdateProperty(Property Property)
        {
          //  if (!Carroll.Data.Services.Helpers.Utility.ValidateFormData(_validationDictionary, FormData)) return Guid.Empty;
            return _repository.CreateUpdateProperty(Property);
            
        }

        public bool DeleteProperty(string PropertyId)
        {
            return _repository.DeleteProperty(PropertyId);
        }

        public List<spProperties_Result> GetProperties(string optionalSeachText = "")
        {
            return _repository.GetProperties(optionalSeachText);
        }

        public Property GetProperty(string PropertyId)
        {
            return _repository.GetProperty(PropertyId);
        }
    }
}
