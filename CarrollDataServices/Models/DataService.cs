using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Services.Models.Validation;


namespace Carroll.Data.Services.Models
{
    public class DataService :IDataService
    {
        private IValidationDictionary _validationDictionary;
        private IDataRepository _repository;
       // private EntityUserRepository entityUserRepository;

        public DataService(IValidationDictionary validationDictionary) 
            : this(validationDictionary, new EntityDataRepository())
        {}

        public DataService(IValidationDictionary validationDictionary, IDataRepository repository)
        {
            _validationDictionary = validationDictionary;
            _repository = repository;
        }


     
        #region [Records]
        public dynamic GetRecords(EntityType entityType, string optionalSeachText = "")
        {
            return  _repository.GetRecords(entityType,optionalSeachText);
        }

        public dynamic GetRecord(EntityType entityType,string recordId)
        {
            return _repository.GetRecord(entityType,recordId);

        }

        public bool CreateUpdateRecord(EntityType entityType,dynamic obj)
        {
            //  if (!Carroll.Data.Services.Helpers.Utility.ValidateFormData(_validationDictionary, FormData)) return Guid.Empty;
            return _repository.CreateUpdateRecord(entityType,obj);

        }

        public bool DeleteRecord(EntityType entityType,string recordId)
        {
            return _repository.DeleteRecord(entityType,recordId);
        }

        public dynamic GetRuntimeClassInstance(string className)
        {
            return _repository.GetRuntimeClassInstance(className);
        }
        #endregion
    }
}
