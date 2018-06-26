
    using System;
    using System.Collections.Generic;
    using System.Linq;

namespace Carroll.Data.Entities.Repository
{
    /// <summary>
    /// Summary description for IAdminRepository
    /// </summary>
    public interface IUserRepository
    {
        bool AuthenticateUser(string User, string Password);

       

        #region [ User Management ]
        List<SiteUser> GetAllUsers(string optionalSeachText = "");
        List<Role> GetAllRoles();

        //bool IsUserNameUnique(string UserName);
        #endregion

        bool CheckIfUserExists(string Email);
        bool CreateUser(SiteUser user);
        SiteUser GetUser(string EmailOrGuid);

        //  List<Contact> GetAllContacts(string optionalSeachText = "");
      
    }
}