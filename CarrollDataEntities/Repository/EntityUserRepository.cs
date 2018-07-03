using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities.Helpers;

namespace Carroll.Data.Entities.Repository
{
    public class EntityUserRepository:IUserRepository
    {
        public CarrollFormsEntities DBEntity => new EFConnectionAccessor().Entities;

        public bool AuthenticateUser(string User, string Pass)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                SiteUser _matches = _entities.SiteUsers.Where(x => x.UserEmail == User && x.UserPassword.Equals(Pass, StringComparison.InvariantCulture) && x.IsActive == true).FirstOrDefault();
                if (_matches != null)
                {
                    if (_matches.UserPassword == Pass) { _matches = null; return true; }
                    else { _matches = null; return false; }
                }
                else return false;
                // return (_matches == 0) ? false : true;
            }
            
        }

        public List<UserInRole> GetUserRoles(Guid userid)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
               // _entities.Configuration.ProxyCreationEnabled = false;
                var _userRoles = _entities.UserInRoles.Where(x => x.UserId == userid).ToList();

                return _userRoles;
            }
        }

       


        public string GetUserRoleName(Guid userid)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                string role = "";
                // _entities.Configuration.ProxyCreationEnabled = false;
                var _userRoles = _entities.UserInRoles.Where(x => x.UserId == userid).ToList();
                foreach (var item in _userRoles)
                {
                    var res = (from tbl in _entities.Roles
                                where tbl.RoleId == item.RoleId
                                select tbl.RoleName).FirstOrDefault();
                    if (role == "")
                        role = res;
                    else
                        role += ", " + res;
                }
                return role;
            }
        }


        public List<SiteUser> GetAllUsers(string optionalSeachText = "")
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                if (string.IsNullOrEmpty(optionalSeachText)) return _entities.SiteUsers.ToList();
                else return _entities.SiteUsers.Where(x => x.FirstName.Contains(optionalSeachText) || x.LastName.Contains(optionalSeachText) || x.UserEmail.Contains(optionalSeachText)).ToList();
                
            }
        }

        public List<Role> GetAllRoles()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                var _roles = _entities.Roles.ToList();

                return _roles;
            }
        }

        public List<Property> GetAllProperties()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                // _entities.Configuration.ProxyCreationEnabled = false;
                _entities.Configuration.ProxyCreationEnabled = false;
                var _properties = _entities.Properties.ToList();

                return _properties;
            }
        }

        #region [ SiteUser Checks ]
        public bool CheckIfUserExists(string Email)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                SiteUser _matches = _entities.SiteUsers.Where(x => x.UserEmail.ToLower() == Email.ToLower()).FirstOrDefault();
                if (_matches != null)
                {
                    return true;

                }
                return false;
            }
        }
        /// <summary>
        /// Returns SiteUser Object
        /// </summary>
        /// <param name="EmailOrGuid"></param>
        /// <returns></returns>
        public SiteUser GetUser(string EmailOrGuid)
        {
            Guid _guid = System.Guid.NewGuid();
            bool IsGuid = false;

            using (CarrollFormsEntities _entities = DBEntity)
            {
                SiteUser _user = null;
                try {
                    // If it is guid it will succeed or it will fail.
                    _guid = new Guid(EmailOrGuid);
                    IsGuid = true;
                }
                catch {
                    IsGuid = false;
                }
                if (IsGuid)
                {
                    _user = _entities.SiteUsers.Where(x => x.UserId == _guid).FirstOrDefault();
                }
                else
                {
                    _user = _entities.SiteUsers.Where(x => x.UserEmail.ToLower() == EmailOrGuid.ToLower()).FirstOrDefault();
                }
                if (_user != null)
                {
                    return _user;

                }
                return null;
            }

        }

        public bool CreateUser(SiteUser user)
        {
            if (!CheckIfUserExists(user.UserEmail))
            {
                // user does not exist let's go ahead and create user
                using (CarrollFormsEntities _entities = DBEntity)
                {
                    // set default's here
                    
                    _entities.SiteUsers.Add(user);
                    int i = _entities.SaveChanges();
                    return (i == 1) ? true : false;
                }

            }
            return false;
        }
        #endregion


       

    }
}
