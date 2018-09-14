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

        public dynamic GetAllClaims(Guid? userid, Guid? propertyid, string optionalSeachText="")
        {
            return _repository.GetAllClaims(userid,propertyid, optionalSeachText);
        }

     public   dynamic GetUserProperty(Guid userid)
        {
            return _repository.GetUserProperty(userid);
        }

        #region [Records]
        public dynamic GetRecords(EntityType entityType, string optionalSeachText = "")
        {
            return  _repository.GetRecords(entityType,optionalSeachText);
        }


        public  dynamic GetRecordsWithConfig(EntityType entityType, string optionalSeachText = "")
        {
            return _repository.GetRecordsWithConfig(entityType, optionalSeachText);
        }

        public dynamic GetRecord(EntityType entityType,string recordId)
        {
            return _repository.GetRecord(entityType,recordId);

        }

        public dynamic CreateUpdateRecord(EntityType entityType,dynamic obj)
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

        public dynamic GetClaimDetails(string Claim, char Type)
        {
            return _repository.GetClaimDetails(Claim,Type);
        }

        public dynamic InsertComment(FormComment obj)
        {
            return _repository.InsertComment(obj);
        }

        public dynamic InsertEmployeeLeaseRider(EmployeeLeaseRaider obj)
        {
            return _repository.InsertEmployeeLeaseRider(obj);
        }

        public dynamic GetEmployeeLeaseRider(Guid obj)
        {
            return _repository.GetEmployeeLeaseRider(obj);
        }

        public dynamic InsertEmployeeNewHireNotice(EmployeeNewHireNotice obj)
        {
            return _repository.InsertEmployeeNewHireNotice(obj);
        }

        public dynamic GetEmployeeNewHireNotice(Guid obj)
        {
            return _repository.GetEmployeeNewHireNotice(obj);
        }


        public dynamic InsertPayRollStatusChangeNotice(PayrollStatusChangeNotice obj)
        {
            return _repository.InsertPayRollStatusChangeNotice(obj);
        }

        public dynamic GetPayRollStatusChangeNotice(Guid obj)
        {
            return _repository.GetPayRollStatusChangeNotice(obj);
        }



        public dynamic InsertNoticeOfEmployeeSeperation(NoticeOfEmployeeSeperation obj)
        {
            return _repository.InsertNoticeOfEmployeeSeperation(obj);
        }

        public dynamic GetNoticeOfEmployeeSeperation(Guid obj)
        {
            return _repository.GetNoticeOfEmployeeSeperation(obj);
        }
        public dynamic GetUserClaimCount(Guid userid)
        {
            return _repository.GetUserClaimCount(userid);
        }

        public dynamic InsertAttachment(FormAttachment formAttachment)
        {
            return _repository.InsertAttachment(formAttachment);
        }

      public  dynamic GetAllHrForms(string FormType, string OptionalSeachText)
        {
            return _repository.GetAllHrForms(FormType, OptionalSeachText);
        }

        public dynamic GetHrFormCount()
        {
            return _repository.GetHrFormCount();
        }
        #endregion
    }
}
