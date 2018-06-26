using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Services.Models.Validation;


namespace Carroll.Data.Services.Models.Contact
{
    public class ContactService :IContactService
    {
        private IValidationDictionary _validationDictionary;
        private IContactRepository _repository;
       // private EntityUserRepository entityUserRepository;

        public ContactService(IValidationDictionary validationDictionary) 
            : this(validationDictionary, new EntityContactRepository())
        {}

        public ContactService(IValidationDictionary validationDictionary, IContactRepository repository)
        {
            _validationDictionary = validationDictionary;
            _repository = repository;
        }


      
        #region [Contacts]
        public List<Carroll.Data.Entities.Contact> GetAllContacts(string optionalSeachText = "")
        {
            return _repository.GetAllContacts(optionalSeachText);
        }

        public Carroll.Data.Entities.Contact GetContact(string ContactId)
        {
            return _repository.GetContact(ContactId);

        }

        public bool CreateUpdateContact(Carroll.Data.Entities.Contact Contact)
        {
            //  if (!Carroll.Data.Services.Helpers.Utility.ValidateFormData(_validationDictionary, FormData)) return Guid.Empty;
            return _repository.CreateUpdateContact(Contact);

        }

        public bool DeleteContact(string ContactId)
        {
            return _repository.DeleteContact(ContactId);
        }
        #endregion
    }
}
