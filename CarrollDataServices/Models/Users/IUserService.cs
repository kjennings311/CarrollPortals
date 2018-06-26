using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
namespace Carroll.Data.Services.Models.Users
{
    public interface IUserService
    {
        #region [ General ]
        bool AuthenticateUser(string User, string Password);
       

        #endregion

        #region [ User Management ]
        List<SiteUser> GetAllUsers(string optionalSeachText = "");

        List<Role> GetAllRoles();


        #endregion

        bool CheckIfUserExists(string Email);
        bool CreateUser(SiteUser user);
        SiteUser GetUser(string EmailOrGuid);
      ///  List<Carroll.Data.Entities.Contact> GetAllContacts(string optionalSeachText = "");
    }
}
