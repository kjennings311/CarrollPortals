using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Services.Models.Validation;
using Carroll.Data.Services.Models.Users;

namespace Carroll.Data.Services.Models.Users
{
    public class UserService :IUserService
    {
        private IValidationDictionary _validationDictionary;
        private IUserRepository _repository;
       // private EntityUserRepository entityUserRepository;

        public UserService(IValidationDictionary validationDictionary) 
            : this(validationDictionary, new EntityUserRepository())
        {}

        public UserService(IValidationDictionary validationDictionary, IUserRepository repository)
        {
            _validationDictionary = validationDictionary;
            _repository = repository;
        }


        public bool AuthenticateUser(string User, string Password)
        {
            return _repository.AuthenticateUser(User, Password);
        }
        

        #region [User Managment]

        public List<SiteUser> GetAllUsers(string optionalSeachText = "")
        {
            return _repository.GetAllUsers(optionalSeachText);
        }

        public List<Role> GetAllRoles()
        {
            return _repository.GetAllRoles();
        }

        public List<Property> GetAllProperties()
        {
            return _repository.GetAllProperties();
        }


        #endregion

        public bool CheckIfUserExists(string Email)
        {
            if (!string.IsNullOrEmpty(Email))
            {
                return _repository.CheckIfUserExists(Email);
            }
            return false;

        }
        public bool CreateUser(SiteUser user)
        {
            if (user != null)
            {
                return _repository.CreateUser(user);
            }
            return false;
        }

        public SiteUser GetUser(string EmailOrGuid)
        {
            if (!string.IsNullOrEmpty(EmailOrGuid))
            {
                return _repository.GetUser(EmailOrGuid);
            }
            return null;
        }
        //#region [Contacts]
        //public List<Carroll.Data.Entities.Contact> GetAllContacts(string optionalSeachText = "")
        //{
        //    return _repository.GetAllContacts(optionalSeachText);
        //}
        //#endregion
    }
}
