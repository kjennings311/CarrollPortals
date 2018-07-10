using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities.Helpers;

namespace Carroll.Data.Entities.Repository
{
    public class EntityDataRepository:IDataRepository
    {
        public CarrollFormsEntities DBEntity => new EFConnectionAccessor().Entities;

        /// <summary>
        /// Generic delete
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public bool DeleteRecord(EntityType entityType,string recordId)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                switch (entityType)
                {
                    case EntityType.Property:
                        // Let's first delete related records first.. then we will delete the main record that will trigger clear in formdata table
                        _entities.Database.ExecuteSqlCommand("DELETE FROM PROPERTIES WHERE PROPERTYID={0} ", recordId);
                        break;
                    case EntityType.Contact:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM CONTACTS WHERE CONTACTID={0} ", recordId);
                        break;
                    case EntityType.Partner:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM EQUITYPARTNERS WHERE EquityPartnerId={0} ", recordId);
                        break;
                    case EntityType.User:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM SiteUsers WHERE UserId={0} ", recordId);
                        break;
                    case EntityType.UserInRole:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM UserInRole WHERE UserRoleId={0} ", recordId);
                        break;
                    case EntityType.UserInProperty:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM UserInProperty WHERE UserInPropertyId={0} ", recordId);
                        break;
                    case EntityType.FormGeneralLiabilityClaim:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM FormGeneralLiabilityClaim WHERE GLLId={0} ", recordId);
                        break;
                    case EntityType.FormMoldDamageClaim:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM FormMoldDamageClaim WHERE MDLId={0} ", recordId);
                        break;
                    case EntityType.FormPropertyDamageClaim:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM FormPropertyDamageClaim WHERE PDLId={0} ", recordId);
                        break;
                    default:
                        break;
                }          

                return true;
            }
        }


        public dynamic GetRecord(EntityType entityType,string recordId)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                Guid _recId = new Guid(recordId);
                
                switch (entityType)
                {

                    case EntityType.Property:
                        #region [ Property ]
                        var _prop = _entities.Properties.Where(x => x.PropertyId == _recId).FirstOrDefault();
                        if (_prop != null) { return _prop; }
                        return null;
                        #endregion
                    case EntityType.Contact:
                        #region [ Cotact ]
                        var _cont = _entities.Contacts.Where(x => x.ContactId == _recId).FirstOrDefault();
                        if (_cont != null) { return _cont; }
                        return null;
                        #endregion
                    case EntityType.Partner:
                        #region [ Equity Partner ]
                        var _partner = _entities.EquityPartners.Where(x => x.EquityPartnerId == _recId).FirstOrDefault();
                        if (_partner != null) { return _partner; }
                        return null;
                        #endregion
                    case EntityType.User:
                        #region [ User ]
                        var _user = _entities.SiteUsers.Where(x => x.UserId == _recId).FirstOrDefault();
                        if (_user != null) { return _user; }
                        return null;
                    #endregion
                    case EntityType.UserInRole:
                        #region [ User In Role ]
                        var _userinrole = _entities.UserInRoles.Where(x => x.UserRoleId == _recId).FirstOrDefault();
                        if (_userinrole != null) { return _userinrole; }
                        return null;
                    #endregion
                    case EntityType.UserInProperty:
                        #region [ User In Property ]
                        var _userinproperty = _entities.UserInProperties.Where(x => x.UserInPropertyId == _recId).FirstOrDefault();
                        if (_userinproperty != null) { return _userinproperty; }
                        return null;
                    #endregion
                    case EntityType.FormPropertyDamageClaim:
                        #region [ FormPropertyDamageClaim]
                        var _damageclaim= _entities.FormPropertyDamageClaims.Where(x => x.PDLId == _recId).FirstOrDefault();
                        if (_damageclaim != null) { return _damageclaim; }
                        return null;
                    #endregion
                    case EntityType.FormGeneralLiabilityClaim:
                        #region [ FormGeneralLiabilityClaim ]
                        var _generalclaim = _entities.FormGeneralLiabilityClaims.Where(x => x.GLLId == _recId).FirstOrDefault();
                        if (_generalclaim != null) { return _generalclaim; }
                        return null;
                    #endregion
                    case EntityType.FormMoldDamageClaim:
                        #region [ FormMoldDamageClaim ]
                        var _formdamageclaim = _entities.FormMoldDamageClaims.Where(x => x.MDLId == _recId).FirstOrDefault();
                        if (_formdamageclaim != null) { return _formdamageclaim; }
                        return null;
                    #endregion
                    default:
                        break;
                }
                return null;
            }

        }

        public bool CreateUpdateRecord(EntityType entityType,dynamic obj)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                try
                {

               
                _entities.Configuration.ProxyCreationEnabled = false;
                switch (entityType)
                {   
                    case EntityType.Property:
                        #region [ Property ]
                        Property _property = obj;
                        var _dbProp = _entities.Properties.Where(x => x.PropertyId == _property.PropertyId).FirstOrDefault();
                        if (_dbProp == null)
                        {
                            if ((_property.PropertyId.ToString() == "00000000-0000-0000-0000-000000000000") || (_property.PropertyId == null))
                            {
                                _property.PropertyId = Guid.NewGuid();
                            }
                            _property.CreatedDate = DateTime.Now;
                            // No record exists create a new property record here
                            _entities.Properties.Add(_property);
                           // _entities.SaveChanges();
                            int i = _entities.SaveChanges();
                            return (i == 1) ? true : false;
                        }
                        else
                        {

                            //_entities.Properties.Attach(Property);
                            //_entities.Entry(Property).State = EntityState.Modified;
                            _entities.Entry(_dbProp).CurrentValues.SetValues(_property);
                            int i = _entities.SaveChanges();
                            // return (i == 1) ? true : false;
                            return true;

                        }
                        #endregion
                    case EntityType.Contact:
                        #region [ Contact ] 
                        Contact _contact = obj;
                        var _dbcontact = _entities.Contacts.Where(x => x.ContactId == _contact.ContactId).FirstOrDefault();
                        if (_dbcontact == null)
                        {
                            if ((_contact.ContactId.ToString() == "00000000-0000-0000-0000-000000000000") || (_contact.ContactId == null))
                            {
                                _contact.ContactId = Guid.NewGuid();
                            }
                            _contact.CreatedDate = DateTime.Now;
                            // No record exists create a new property record here
                            
                            _entities.Contacts.Add(_contact);
                            _entities.SaveChanges();
                            int i = _entities.SaveChanges();
                            // return (i == 1) ? true : false;
                            return true;
                        }
                        else
                        {
                            _entities.Entry(_dbcontact).CurrentValues.SetValues(_contact);
                            int i = _entities.SaveChanges();
                            return true;
                        }
                    #endregion
                    case EntityType.Partner:
                        #region [ Partner ] 
                        EquityPartner _partner = obj;
                        var _dbpartner = _entities.EquityPartners.Where(x => x.EquityPartnerId == _partner.EquityPartnerId).FirstOrDefault();
                        if (_dbpartner == null)
                        {
                            if ((_partner.EquityPartnerId.ToString() == "00000000-0000-0000-0000-000000000000") || (_partner.EquityPartnerId == null))
                            {
                                _partner.EquityPartnerId = Guid.NewGuid();
                            }
                            _partner.CreatedDate = DateTime.Now;
                            // No record exists create a new property record here

                            _entities.EquityPartners.Add(_partner);
                            _entities.SaveChanges();
                            int i = _entities.SaveChanges();
                            // return (i == 1) ? true : false;
                            return true;
                        }
                        else
                        {

                            _entities.Entry(_dbpartner).CurrentValues.SetValues(_partner);
                            int i = _entities.SaveChanges();
                            return true;

                        }
                    #endregion

                    case EntityType.User:
                        #region [ User ] 
                        
                        SiteUser _user = obj;
                        var _dbuser = _entities.SiteUsers.Where(x => x.UserId == _user.UserId).FirstOrDefault();
                        if (_dbuser == null)
                        {
                            if ((_user.UserId.ToString() == "00000000-0000-0000-0000-000000000000") || (_user.UserId == null))
                            {
                                _user.UserId = Guid.NewGuid();
                            }
                            _user.CreatedDate = DateTime.Now;
                            // No record exists create a new property record here

                            _entities.SiteUsers.Add(_user);
                            _entities.SaveChanges();
                            int i = _entities.SaveChanges();
                            // return (i == 1) ? true : false;
                            return true;
                        }
                        else
                        {
                            _user.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbuser).CurrentValues.SetValues(_user);
                            int i = _entities.SaveChanges();
                            return true;
                        }
                    #endregion

                    case EntityType.UserInRole:
                        #region [ User Roles ] 

                        UserInRole _userrole = obj;
                        var _dbuserinrole = _entities.UserInRoles.Where(x => x.UserRoleId == _userrole.UserRoleId).FirstOrDefault();
                        if (_dbuserinrole == null)
                        {
                            if ((_userrole.UserRoleId.ToString() == "00000000-0000-0000-0000-000000000000") || (_userrole.UserRoleId == null))
                            {
                                _userrole.UserRoleId = Guid.NewGuid();
                            }
                            _userrole.CreatedDate = DateTime.Now;
                            // No record exists create a new property record here

                            _entities.UserInRoles.Add(_userrole);
                            _entities.SaveChanges();
                            int i = _entities.SaveChanges();
                            // return (i == 1) ? true : false;
                            return true;
                        }
                        else
                        {
                            _userrole.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbuserinrole).CurrentValues.SetValues(_userrole);
                            int i = _entities.SaveChanges();
                            return true;
                        }
                    #endregion

                    case EntityType.UserInProperty:
                        #region [ User Property ] 

                        UserInProperty _userProp = obj;
                        var _dbuserproperty = _entities.UserInProperties.Where(x => x.UserInPropertyId == _userProp.UserInPropertyId).FirstOrDefault();
                        if (_dbuserproperty == null)
                        {
                            if ((_userProp.UserInPropertyId.ToString() == "00000000-0000-0000-0000-000000000000") || (_userProp.UserInPropertyId == null))
                            {
                                _userProp.UserInPropertyId = Guid.NewGuid();
                            }
                            _userProp.CreatedDate = DateTime.Now;
                            // No record exists create a new property record here

                            _entities.UserInProperties.Add(_userProp);
                            _entities.SaveChanges();
                            int i = _entities.SaveChanges();
                          
                            // return (i == 1) ? true : false;
                            return true;
                        }
                        else
                        {
                            _userProp.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbuserproperty).CurrentValues.SetValues(_userProp);
                            int i = _entities.SaveChanges();
                            return true;
                        }
                    #endregion

                    case EntityType.FormGeneralLiabilityClaim:
                        #region [ FormGeneralLiabilityClaim ] 

                        FormGeneralLiabilityClaim _glc = obj;
                        var _dbglc = _entities.FormGeneralLiabilityClaims.Where(x => x.GLLId == _glc.GLLId).FirstOrDefault();
                        if (_dbglc == null)
                        {
                            if ((_glc.GLLId.ToString() == "00000000-0000-0000-0000-000000000000") || (_glc.GLLId == null))
                            {
                                _glc.GLLId = Guid.NewGuid();
                            }
                            _glc.CreatedDate = DateTime.Now;
                            // No record exists create a new property record here

                            _entities.FormGeneralLiabilityClaims.Add(_glc);
                            _entities.SaveChanges();
                            int i = _entities.SaveChanges();

                            // return (i == 1) ? true : false;
                            return true;
                        }
                        else
                        {
                            _glc.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbglc).CurrentValues.SetValues(_glc);
                            int i = _entities.SaveChanges();
                            return true;
                        }

                    #endregion

                    case EntityType.FormMoldDamageClaim:
                        #region [ FormMoldDamageClaim ] 

                        FormMoldDamageClaim _mdc = obj;
                        var _dbmdc = _entities.FormMoldDamageClaims.Where(x => x.MDLId == _mdc.MDLId).FirstOrDefault();
                        if (_dbmdc == null)
                        {
                            if ((_mdc.MDLId.ToString() == "00000000-0000-0000-0000-000000000000") || (_mdc.MDLId == null))
                            {
                                _mdc.MDLId = Guid.NewGuid();
                            }
                            _mdc.CreatedDate = DateTime.Now;
                            // No record exists create a new property record here

                            _entities.FormMoldDamageClaims.Add(_mdc);
                            _entities.SaveChanges();
                            int i = _entities.SaveChanges();

                            // return (i == 1) ? true : false;
                            return true;
                        }
                        else
                        {
                            _mdc.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbmdc).CurrentValues.SetValues(_mdc);
                            int i = _entities.SaveChanges();
                            return true;
                        }
                    #endregion

                    case EntityType.FormPropertyDamageClaim:
                        #region [ FormPropertyDamageClaim ] 

                        FormPropertyDamageClaim _pdc = obj;
                        var _dbpdc = _entities.FormPropertyDamageClaims.Where(x => x.PDLId == _pdc.PDLId).FirstOrDefault();
                        if (_dbpdc == null)
                        {
                            if ((_pdc.PDLId.ToString() == "00000000-0000-0000-0000-000000000000") || (_pdc.PDLId == null))
                            {
                                _pdc.PDLId = Guid.NewGuid();
                            }
                            _pdc.CreatedDate = DateTime.Now;
                            // No record exists create a new property record here

                            _entities.FormPropertyDamageClaims.Add(_pdc);
                            _entities.SaveChanges();
                            int i = _entities.SaveChanges();

                            // return (i == 1) ? true : false;
                            return true;
                        }
                        else
                        {
                            _pdc.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbpdc).CurrentValues.SetValues(_pdc);
                            int i = _entities.SaveChanges();
                            return true;
                        }
                    #endregion
                    default:
                        // if we are here that means it must be a dynamic object.. Let's try to evaluate and insert..

                        Type _objType = obj?.GetType();

                        break;
                }
                }
                catch (Exception ex)
                {

                    return false;
                }

                return true;
            }

        }

        public dynamic GetRecords(EntityType entityType,string optionalSeachText = "")
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
               
                switch (entityType)
                {
                    case EntityType.Property:
                        #region [ Property ]
                        // we are calling stored procedure spProperties_Result here..
                        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.spProperties().ToList();
                        else return _entities.spProperties().Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.LegalName.ToLower().Contains(optionalSeachText.ToLower())).ToList();
                        #endregion
                    case EntityType.Contact:
                        #region [ Contact ]
                        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.Contacts.ToList();
                        else return _entities.Contacts.Where(x => x.IsActive && (x.FirstName.Contains(optionalSeachText) || x.LastName.Contains(optionalSeachText) || x.Title.Contains(optionalSeachText) || x.Phone.Contains(optionalSeachText) || x.Email.Contains(optionalSeachText))).ToList();
                        #endregion
                    case EntityType.Partner:
                        #region
                        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.EquityPartners.ToList();
                        else return _entities.EquityPartners.Where(x => x.IsActive && (x.PartnerName.Contains(optionalSeachText) || x.AddressLine1.Contains(optionalSeachText) || x.AddressLine2.Contains(optionalSeachText) || x.City.Contains(optionalSeachText) || x.State.Contains(optionalSeachText))).ToList();
                    #endregion
                    case EntityType.UserInRole:
                        #region [ User In Role ]
                        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.sp_GetUserRoles().ToList();
                        else return _entities.sp_GetUserRoles().Where(x => x.userName.Contains(optionalSeachText) || x.UserEmail.Contains(optionalSeachText) || x.RoleName.Contains(optionalSeachText)).ToList();
                    #endregion
                    case EntityType.UserInProperty:
                        #region [ User In Property ]
                        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.sp_GetUserProperties().ToList();
                        else return _entities.sp_GetUserProperties().Where(x => x.userName.Contains(optionalSeachText) || x.UserEmail.Contains(optionalSeachText) || x.PropertyName.Contains(optionalSeachText) || x.PropertyAddress.Contains(optionalSeachText) || x.City.Contains(optionalSeachText)).ToList();
                    #endregion
                    case EntityType.User:
                        #region [ User ]
                        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.SiteUsers.ToList();
                        else return _entities.SiteUsers.Where(x => x.IsActive && (x.FirstName.Contains(optionalSeachText) || x.LastName.Contains(optionalSeachText) ||  x.Phone.Contains(optionalSeachText) || x.UserEmail.Contains(optionalSeachText))).ToList();
                    #endregion
                    case EntityType.FormPropertyDamageClaim:
                        #region [ FormPropertyDamageClaim ]
                        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.proc_GetPropertyDamageClaims().ToList();
                        else return _entities.proc_GetPropertyDamageClaims().Where(x => x.PropertyName.Contains(optionalSeachText) || x.IncidentLocation.Contains(optionalSeachText) || x.EstimateOfDamage.Contains(optionalSeachText) || x.WeatherConditions.Contains(optionalSeachText)).ToList();
                    #endregion
                    case EntityType.FormMoldDamageClaim:
                        #region [ FormPropertyDamageClaim ]
                        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.proc_GetMoldDamageClaims().ToList();
                        else return _entities.proc_GetMoldDamageClaims().Where(x => x.PropertyName.Contains(optionalSeachText) || x.Description.Contains(optionalSeachText) || x.Location.Contains(optionalSeachText) || x.EstimatedTimeDamagePresent.Contains(optionalSeachText)).ToList();
                    #endregion

                    case EntityType.FormGeneralLiabilityClaim:
                        #region [ FormGeneralLiabilityClaim ]
                        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.proc_GetGeneralLiabilityClaims().ToList();
                        else return _entities.proc_GetGeneralLiabilityClaims().Where(x => x.PropertyName.Contains(optionalSeachText) || x.IncidentLocation.Contains(optionalSeachText) || x.IncidentDescription.Contains(optionalSeachText) || x.IncidentDateTime.ToString().Contains(optionalSeachText)).ToList();
                    #endregion

                    default:
                        break;
                }

            }
            return null;

        }

        public dynamic GetRecordsWithConfig(EntityType entityType, string optionalSeachText = "")
        {
             using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;

                var config = new Config { };

                switch (entityType)
                {

                    case EntityType.Property:

                    //    #region [ Property ]

                    //    // we are calling stored procedure spProperties_Result here..
                    //    if (string.IsNullOrEmpty(optionalSeachText))
                    //         config.Rows=_entities.spProperties().ToList();
                    //    else
                    //        config.Rows = _entities.spProperties().Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.LegalName.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    //    config.EtType = entityType.ToString();
                    //    PropertyInfo[] properties = typeof(spProperties_Result).GetProperties();
                    //    config.PkName = FirstChartoLower(properties.ToList().FirstOrDefault().Name);
                    //    config.Columns = new List<DtableConfigArray>();

                    //    foreach (var item in properties)
                    //    {
                    //        config.Columns.Add(new DtableConfigArray { data = FirstChartoLower(item.Name), name = FirstChartoLower(item.Name), autoWidth = false });
                    //    }

                    //    return config;

                    //#endregion

                    case EntityType.User:

                        #region [User]

                        // we are calling stored procedure spProperties_Result here..
                        if (string.IsNullOrEmpty(optionalSeachText))
                            config.Rows = _entities.SiteUsers.ToList();
                        else
                            config.Rows = _entities.SiteUsers.Where(x => x.FirstName.ToLower().Contains(optionalSeachText.ToLower()) || x.LastName.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                        config.EtType = entityType.ToString();
                        PropertyInfo[] userprop = typeof(SiteUser).GetProperties();
                        config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                        config.Columns = new List<DtableConfigArray>();

                        config.Columns.Add(new DtableConfigArray { name = "userEmail", type=0,href="" });
                        config.Columns.Add(new DtableConfigArray { name = "firstName", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "lastName", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "phone", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "userPhoto", type = DFieldType.IsPic, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "isActive", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "isApproved", type = 0, href = "" });

                        return config;

                    #endregion
                    default:
                        break;
                }

            }
            return null;

        }

        // ************************ STORED PROCEDURES ****************************************//
        //public List<spProperties_Result> GetProperties(string optionalSeachText = "")
        //{
        //    using (CarrollFormsEntities _entities = DBEntity)
        //    {
        //        _entities.Configuration.ProxyCreationEnabled = false;
        //        if (string.IsNullOrEmpty(optionalSeachText)) return _entities.spProperties().ToList();
        //        else return _entities.spProperties().Where(x => x.PropertyName.Contains(optionalSeachText) || x.LegalName.Contains(optionalSeachText)).ToList();

        //    }
        //}

        public dynamic GetRuntimeClassInstance(string className)
        {
            if (className == "User")
                className = "SiteUser";

            var type = Type.GetType("Carroll.Data.Entities." + className);
            return Activator.CreateInstance(type);

        }

        public string FirstChartoLower(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToLower() + input.Substring(1);
        }
        //public Config GetDatatableConfig(EntityType entityType,)
        //{

        //    config.EtType = entityType.ToString();
        //    PropertyInfo[] properties = typeof(spProperties_Result).GetProperties();
        //    config.PkName = FirstChartoLower(properties.ToList().FirstOrDefault().Name);
        //    config.Columns = new List<DtableConfigArray>();

        //    foreach (var item in properties)
        //    {
        //        config.Columns.Add(new DtableConfigArray { data = FirstChartoLower(item.Name), name = FirstChartoLower(item.Name), autoWidth = false });
        //    }
        //}
    }
}
