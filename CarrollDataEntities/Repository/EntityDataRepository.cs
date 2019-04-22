using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        /// 
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
                    case EntityType.PayPeriods:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM CarrollPayPeriods WHERE PeriodId={0} ", recordId);
                        break;
                    case EntityType.CarrollPositions:
                        _entities.Database.ExecuteSqlCommand("DELETE FROM CarrollPositions WHERE PositionId={0} ", recordId);
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
                        #region [ Contact ]
                        var _cont = _entities.Contacts.Where(x => x.ContactId == _recId).FirstOrDefault();
                        if (_cont != null) { return _cont; }
                        return null;
                    #endregion
                    case EntityType.CarrollPositions: 
                        #region [ carrollpositions ]
                        var _pos = _entities.CarrollPositions.Where(x => x.PositionId == _recId).FirstOrDefault();
                        if (_pos != null) { return _pos; }
                        return null;
                    #endregion
                    case EntityType.PayPeriods:
                        #region [ carrollpositions ]
                        var _per = _entities.CarrollPayPeriods.Where(x => x.PeriodId == _recId).FirstOrDefault();
                        if (_per != null) { return _per; }
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

        public dynamic CreateUpdateRecord(EntityType entityType,dynamic obj)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                RecordUpdateResult _result = new RecordUpdateResult();
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
                            bool bSuccess = true;
                            _result.RecordId = _property.PropertyId.ToString();
                            _result.Succeded = bSuccess;

                            return _result;
                        }
                        else
                        {

                            //_entities.Properties.Attach(Property);
                            //_entities.Entry(Property).State = EntityState.Modified;
                            _entities.Entry(_dbProp).CurrentValues.SetValues(_property);
                            int i = _entities.SaveChanges();
                                // return (i == 1) ? true : false;
                                _result.RecordId = _property.PropertyId.ToString();
                                _result.Succeded = true;

                                return _result;


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
                                _result.RecordId = _contact.ContactId.ToString();
                                _result.Succeded = true;

                                return _result;
                                // return (i == 1) ? true : false;
                                // return true;
                            }
                        else
                        {
                            _entities.Entry(_dbcontact).CurrentValues.SetValues(_contact);
                            int i = _entities.SaveChanges();
                                _result.RecordId = _contact.ContactId.ToString();
                                _result.Succeded = true;

                                return _result;
                                // return true;
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
                            //
                            // No record exists create a new property record here

                            _entities.EquityPartners.Add(_partner);
                            _entities.SaveChanges();
                            int i = _entities.SaveChanges();
                                // return (i == 1) ? true : false;
                                //return true;
                                _result.RecordId = _partner.EquityPartnerId.ToString();
                                _result.Succeded = true;

                                return _result;
                            }
                        else
                        {

                            _entities.Entry(_dbpartner).CurrentValues.SetValues(_partner);
                            int i = _entities.SaveChanges();
                                // return true;
                                _result.RecordId = _partner.EquityPartnerId.ToString();
                                _result.Succeded = true;

                                return _result;

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
                                // return true;
                                //return true;
                                _result.RecordId = _user.UserId.ToString();
                                _result.Succeded = true;

                                return _result;
                            }
                        else
                        {
                            _user.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbuser).CurrentValues.SetValues(_user);
                            int i = _entities.SaveChanges();
                                _result.RecordId = _user.UserId.ToString();
                                _result.Succeded = true;

                                return _result;
                                // return true;
                            }
                        #endregion


                        case EntityType.PayPeriods:
                            #region [ PayPeriods ] 

                            CarrollPayPeriod _payperiod = obj;
                            var _dbpayperild = _entities.CarrollPayPeriods.Where(x => x.PeriodId == _payperiod.PeriodId).FirstOrDefault();
                            if (_dbpayperild == null)
                            {
                                if ((_payperiod.PeriodId.ToString() == "00000000-0000-0000-0000-000000000000") || (_payperiod.PeriodId == null))
                                {
                                    _payperiod.PeriodId = Guid.NewGuid();
                                }
                                _payperiod.CreatedDate = DateTime.Now;
                                // No record exists create a new property record here

                                _entities.CarrollPayPeriods.Add(_payperiod);
                                _entities.SaveChanges();
                                int i = _entities.SaveChanges();
                                // return true;
                                //return true;
                                _result.RecordId = _payperiod.PeriodId.ToString();
                                _result.Succeded = true;

                                return _result;
                            }
                            else
                            {
                                _payperiod.ModifiedDate = DateTime.Now;
                                _entities.Entry(_dbpayperild).CurrentValues.SetValues(_payperiod);
                                int i = _entities.SaveChanges();
                                _result.RecordId = _payperiod.PeriodId.ToString();
                                _result.Succeded = true;

                                return _result;
                                // return true;
                            }
                        #endregion



                        case EntityType.CarrollPositions:
                            #region [ CarrollPositions ] 

                            CarrollPosition _cp = obj;
                            var _dbcp = _entities.CarrollPositions.Where(x => x.PositionId == _cp.PositionId).FirstOrDefault();
                            if (_dbcp == null)
                            {
                                if ((_cp.PositionId.ToString() == "00000000-0000-0000-0000-000000000000") || (_cp.PositionId == null))
                                {
                                    _cp.PositionId = Guid.NewGuid();
                                }
                                _cp.CreatedDate = DateTime.Now;
                                // No record exists create a new property record here

                                _entities.CarrollPositions.Add(_cp);
                                _entities.SaveChanges();
                                int i = _entities.SaveChanges();
                                // return true;
                                //return true;
                                _result.RecordId = _cp.PositionId.ToString();
                                _result.Succeded = true;

                                return _result;
                            }
                            else
                            {
                                _cp.ModifiedDat = DateTime.Now;
                                _entities.Entry(_dbcp).CurrentValues.SetValues(_cp);
                                int i = _entities.SaveChanges();
                                _result.RecordId = _cp.PositionId.ToString();
                                _result.Succeded = true;

                                return _result;
                                // return true;
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
                                // return true;
                                _result.RecordId = _userrole.UserRoleId.ToString();
                                _result.Succeded = true;

                                return _result;
                            }
                        else
                        {
                            _userrole.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbuserinrole).CurrentValues.SetValues(_userrole);
                            int i = _entities.SaveChanges();
                                //return true;
                                _result.RecordId = _userrole.UserRoleId.ToString();
                                _result.Succeded = true;

                                return _result;
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
                                // return true;
                                _result.RecordId = _userProp.UserInPropertyId.ToString();
                                _result.Succeded = true;

                                return _result;
                            }
                        else
                        {
                            _userProp.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbuserproperty).CurrentValues.SetValues(_userProp);
                            int i = _entities.SaveChanges();
                                //return true;
                                _result.RecordId = _userProp.UserInPropertyId.ToString();
                                _result.Succeded = true;

                                return _result;
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

                                string Comment = "FormGeneralLiabilityClaim Record was added on " + _glc.CreatedDate.ToString();
                                LogActivity(Comment, _glc.CreatedByName, _glc.CreatedBy.ToString(), _glc.GLLId.ToString(), "New GL Claim");
                                // return (i == 1) ? true : false;
                                // return true;
                                _result.RecordId = _glc.GLLId.ToString();
                                _result.Succeded = true;

                                return _result;
                            }
                        else
                        {
                            //_glc.mod = DateTime.Now;
                            _entities.Entry(_dbglc).CurrentValues.SetValues(_glc);
                            int i = _entities.SaveChanges();
                                // return true;
                                _result.RecordId = _glc.GLLId.ToString();
                                _result.Succeded = true;

                                return _result;
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
                                string Comment = "Mold Damage Claim Record was added on " + _mdc.CreatedDate.ToString();
                                LogActivity(Comment, _mdc.CreatedByName, _mdc.CreatedBy.ToString(), _mdc.MDLId.ToString(), "New MD Claim");
                                // return (i == 1) ? true : false;
                                // return true;
                                _result.RecordId = _mdc.MDLId.ToString();
                                _result.Succeded = true;

                                return _result;
                            }
                        else
                        {
                            _mdc.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbmdc).CurrentValues.SetValues(_mdc);
                            int i = _entities.SaveChanges();
                                //return true;
                                _result.RecordId = _mdc.MDLId.ToString();
                                _result.Succeded = true;

                                return _result;
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


                            string Comment = "Property Damage Claim Record was added on " + _pdc.CreatedDate.ToString();
                            LogActivity(Comment, _pdc.CreatedByName, _pdc.CreatedBy.ToString(), _pdc.PDLId.ToString(), "New PD Claim");
                                // return (i == 1) ? true : false;
                                //return true;
                                _result.RecordId = _pdc.PDLId.ToString();
                                _result.Succeded = true;

                                return _result;
                            }
                        else
                        {
                            _pdc.CreatedDate = DateTime.Now;
                            _entities.Entry(_dbpdc).CurrentValues.SetValues(_pdc);
                            int i = _entities.SaveChanges();
                                //return true;
                                _result.RecordId = _pdc.PDLId.ToString();
                                _result.Succeded = true;

                                return _result;
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

                        BaseRepository<Contact> rep = new BaseRepository<Contact>(_entities);
                       
                        List<Contact>  _result =  rep.GetAll().ToList();
                        if (!string.IsNullOrEmpty(optionalSeachText))
                        {
                            dynamic _temp = _result.Where(s => s.FirstName.ToLower().Contains(optionalSeachText) || s.LastName.ToLower().Contains(optionalSeachText)).ToList();
                            return _temp;
                        }
                        return _result;
                        //if (string.IsNullOrEmpty(optionalSeachText)) return _entities.Contacts.ToList();
                        //else return _entities.Contacts.Where(x => x.IsActive && (x.FirstName.Contains(optionalSeachText) || x.LastName.Contains(optionalSeachText) || x.Title.Contains(optionalSeachText) || x.Phone.Contains(optionalSeachText) || x.Email.Contains(optionalSeachText))).ToList();
                        #endregion

                    case EntityType.Partner :

                        #region [Partner]

                        if (string.IsNullOrEmpty(optionalSeachText))
                        {
                            return (from tbl in _entities.EquityPartners
                                       join tblcontact in _entities.Contacts on tbl.ContactId equals tblcontact.ContactId
                                       select new { equityPartnerId=tbl.EquityPartnerId,  partnerName = tbl.PartnerName, addressLine1 = tbl.AddressLine1, addressLine2 = tbl.AddressLine2, city = tbl.City, state = tbl.State, zipCode = tbl.ZipCode,contactId=tblcontact.FirstName+" "+tblcontact.LastName,createdDate=tbl.CreatedDate, createdByName=tbl.CreatedByName }).ToList();

                        }
                        else
                        {
                          return  (from tbl in _entities.EquityPartners
                                       join tblcontact in _entities.Contacts on tbl.ContactId equals tblcontact.ContactId
                                       where tbl.IsActive== true && ( tbl.PartnerName.Contains(optionalSeachText) || tbl.AddressLine1.Contains(optionalSeachText) || tbl.AddressLine2.Contains(optionalSeachText) ||  tbl.City.Contains(optionalSeachText ) || tbl.State.Contains(optionalSeachText))
                                       select new { equityPartnerId = tbl.EquityPartnerId, partnerName = tbl.PartnerName, addressLine1 = tbl.AddressLine1, addressLine2 = tbl.AddressLine2, city = tbl.City, state = tbl.State, zipCode = tbl.ZipCode, contactId = tblcontact.FirstName + " " + tblcontact.LastName, createdDate = tbl.CreatedDate, createdByName = tbl.CreatedByName }).ToList();
                        }


                        //if (string.IsNullOrEmpty(optionalSeachText))
                        //    return _entities.EquityPartners.ToList();
                        //else return _entities.EquityPartners.Where(x => x.IsActive && (x.PartnerName.Contains(optionalSeachText) || x.AddressLine1.Contains(optionalSeachText) || x.AddressLine2.Contains(optionalSeachText) || x.City.Contains(optionalSeachText) || x.State.Contains(optionalSeachText))).ToList();

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


        public dynamic GetUserClaimCount(Guid userid)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {


                var ClaimCounts = new { PropertyCount = 0, DamangeCount = 0, LiabilityCount = 0 };

                var res = (from tbl in _entities.Roles
                           join tbluserrole in _entities.UserInRoles on tbl.RoleId equals tbluserrole.RoleId
                           where tbluserrole.UserId == userid && tbl.RoleName.Contains("admin")
                           select tbl).Count();

                if(res == 1)
                {

                    var _propcount = (from tbl in _entities.FormPropertyDamageClaims    
                                      join tbluser in _entities.SiteUsers on tbl.CreatedBy equals tbluser.UserId
                                      select tbl).Count();
                    var _damagecount = (from tbl in _entities.FormMoldDamageClaims
                                        join tbluser in _entities.SiteUsers on tbl.CreatedBy equals tbluser.UserId
                                        select tbl).Count();
                    var _liabilitycount = (from tbl in _entities.FormGeneralLiabilityClaims
                                           join tbluser in _entities.SiteUsers on tbl.CreatedBy equals tbluser.UserId
                                           select tbl).Count();

                    return new { PropertyCount = _propcount, DamageCount = _damagecount, LiabilityCount = _liabilitycount };
                }
                else
                {
                    var _propcount = (from tbl in _entities.FormPropertyDamageClaims
                                      where tbl.CreatedBy == userid
                                      select tbl).Count();
                    var _damagecount = (from tbl in _entities.FormMoldDamageClaims
                                        where tbl.CreatedBy == userid
                                        select tbl).Count();
                    var _liabilitycount = (from tbl in _entities.FormGeneralLiabilityClaims
                                           where tbl.CreatedBy == userid
                                           select tbl).Count();
                    return new { PropertyCount = _propcount, DamageCount = _damagecount, LiabilityCount = _liabilitycount };
                }                
            }
        }

        public dynamic GetUserProperty(Guid userid)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                var res = (from tbl in _entities.UserInProperties
                           where tbl.UserId == userid
                           select tbl.PropertyId).ToList();
                var str= "";

                foreach (var item in res)
                {
                    if(str!="")
                    str += item.ToString();
                    else
                    str +=","+ item.ToString();
                }

                return res;
            }
        }

        public dynamic GetEquityPartners()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {

                var res = (from tbl in _entities.EquityPartners
                           join tblcontact in _entities.Contacts on tbl.ContactId equals tblcontact.ContactId
                           where tbl.IsActive == true
                           select tbl).ToList();

                return res;
            }
        }

        

        public dynamic GetAllClaims(Guid? userid,Guid? propertyid,string Type, string optionalSeachText)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;

                var config = new Config { };

                        if (string.IsNullOrEmpty(optionalSeachText)  && string.IsNullOrEmpty(Type))
                            config.Rows = _entities.SP_GetAllClaims1(userid,propertyid).ToList();
                        else if(Type=="All")
                            config.Rows = _entities.SP_GetAllClaims1(userid, propertyid).Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentLocation.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentDescription.ToLower().Contains(optionalSeachText.ToLower()) || x.ReportedBy.ToLower().Contains(optionalSeachText.ToLower())).ToList();
                        else
                    config.Rows = _entities.SP_GetAllClaims1(userid, propertyid).Where(x => (x.ClaimType.ToLower() == Type.ToLower() && (x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentLocation.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentDescription.ToLower().Contains(optionalSeachText.ToLower()) || x.ReportedBy.ToLower().Contains(optionalSeachText.ToLower())))).ToList();

                config.EtType = EntityType.AllClaims.ToString();
                        PropertyInfo[] userprop = typeof(SP_GetAllClaims1_Result).GetProperties();
                        config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                        config.Columns = new List<DtableConfigArray>();
                config.Columns.Add(new DtableConfigArray { name = "claimNumber", label = "Claim Number", type = 0, href = "" });

                config.Columns.Add(new DtableConfigArray { name = "claimType", label = "Claim Type", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "propertyName", label = "Property Name", type = 0, href = "" });
                       // config.Columns.Add(new DtableConfigArray { name = "propertyNumber", label = "Property Number", type = 0, href = "" });
                       // config.Columns.Add(new DtableConfigArray { name = "propertyManager", label = "Property Manager", type = 0, href = "" });
                     //   config.Columns.Add(new DtableConfigArray { name = "propertyAddress", label = "Property Address", type = 0, href = "" });
                    //    config.Columns.Add(new DtableConfigArray { name = "incidentLocation", label = "Incident Location", type = 0, href = "" });
                      //  config.Columns.Add(new DtableConfigArray { name = "incidentDescription", label = "Incident Description", type = 0, href = "" });
                config.Columns.Add(new DtableConfigArray { name = "incidentDateTime", label = "Incident Date", type = DFieldType.IsDate, href = "" });
               // config.Columns.Add(new DtableConfigArray { name = "reportedBy", label = "Reported By", type = 0, href = "" });
                //config.Columns.Add(new DtableConfigArray { name = "dateReported", label = "Date Reported", type = DFieldType.IsDate, href = "" });
                //config.Columns.Add(new DtableConfigArray { name = "reportedPhone", label = "Reported Phone", type = DFieldType.IsText, href = "" });
                //config.Columns.Add(new DtableConfigArray { name = "createdDate", label = "Created Date", type =  DFieldType.IsDate, href = "" });

                return config;                

            }

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

                        config.Columns.Add(new DtableConfigArray { name = "userEmail", label="User Email", type=0,href="" });
                        config.Columns.Add(new DtableConfigArray { name = "firstName", label = "First Name", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "lastName", label = "Last Name", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "phone", label = "Phone", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "userPhoto", label = "User Photo", type = DFieldType.IsPic, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "isActive", label = "Is Active", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "isApproved", label = "Is Approved", type = 0, href = "" });

                        return config;

                    #endregion
                    case EntityType.PayPeriods:

                        #region [PayPeriod]

                        // we are calling stored procedure spProperties_Result here..
                        if (string.IsNullOrEmpty(optionalSeachText))
                            config.Rows = _entities.proc_getallcarrollpayperiods().ToList();
                        else
                            config.Rows = _entities.proc_getallcarrollpayperiods().Where(x => x.PayFrom.Value.ToString().ToLower().Contains(optionalSeachText.ToLower()) || x.PayTo.Value.ToString().ToLower().Contains(optionalSeachText.ToLower())).ToList();

                        config.EtType = entityType.ToString();
                        PropertyInfo[] userprop1 = typeof(CarrollPayPeriod).GetProperties();
                        config.PkName = FirstChartoLower(userprop1.ToList().FirstOrDefault().Name);
                        config.Columns = new List<DtableConfigArray>();

                        config.Columns.Add(new DtableConfigArray { name = "payFrom", label = "Pay From", type = DFieldType.IsDate, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "payTo", label = "Pay To", type = DFieldType.IsDate, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "createdDate", label = "Created Date", type = DFieldType.IsDate, href = "" });
                      

                        return config;

                    #endregion

                    case EntityType.CarrollPositions:

                        #region [PayPeriod]

                        // we are calling stored procedure spProperties_Result here..
                        if (string.IsNullOrEmpty(optionalSeachText))
                            config.Rows = _entities.proc_getallcarrollpositions().ToList();
                        else
                            config.Rows = _entities.proc_getallcarrollpositions().Where(x => x.Position.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                        config.EtType = entityType.ToString();
                        PropertyInfo[] userprop11 = typeof(CarrollPosition).GetProperties();
                        config.PkName = FirstChartoLower(userprop11.ToList().FirstOrDefault().Name);
                        config.Columns = new List<DtableConfigArray>();

                        config.Columns.Add(new DtableConfigArray { name = "position", label = "Position", type = 0, href = "" });
                     //   config.Columns.Add(new DtableConfigArray { name = "payTo", label = "Pay To", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                        config.Columns.Add(new DtableConfigArray { name = "createdDate", label = "Created Date", type = DFieldType.IsDate, href = "" });


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

        public dynamic GetClaimDetails(string Claim, char Type)
        {
            ClaimDetails cd = new ClaimDetails();

            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                Guid _recId = new Guid(Claim);
                Int16 formtype = 1;

                if (Type == 'g')
                {
                    //  var _generalclaim = _entities.FormGeneralLiabilityClaims.Where(x => x.GLLId == _recId).FirstOrDefault();

                    var _generalclaim = (from tbl in _entities.FormGeneralLiabilityClaims
                                         join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                         where tbl.GLLId == _recId
                                         select new { tbl, tblprop.PropertyName }).FirstOrDefault();
                    if (_generalclaim != null)
                    { cd.Claim = _generalclaim; }
                    formtype = 2;

                }
                else if (Type == 'm')
                {
                    var _formdamageclaim = (from tbl in _entities.FormMoldDamageClaims
                                            join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                            where tbl.MDLId == _recId
                                            select new { tbl, tblprop.PropertyName }).FirstOrDefault();

                    //_entities.FormMoldDamageClaims.Where(x => x.MDLId == _recId).FirstOrDefault();
                    if (_formdamageclaim != null)
                    { cd.Claim = _formdamageclaim; }
                    formtype = 3;



                }
                else if (Type == 'p')
                {
                    var _damageclaim = (from tbl in _entities.FormPropertyDamageClaims
                                        join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                        where tbl.PDLId == _recId
                                        select new { tbl, tblprop.PropertyName }).FirstOrDefault();

                    //_entities.FormPropertyDamageClaims.Where(x => x.PDLId == _recId).FirstOrDefault();
                    if (_damageclaim != null) { cd.Claim = _damageclaim; }
                    formtype = 1;

                }

                // Get All Comments for this RowId and Type

                var AllComments = (from tbl in _entities.FormComments
                                   where tbl.RefFormID == _recId && tbl.RefFormType == formtype orderby
                                   tbl.CommentDate descending
                                   select tbl).ToList();

                // Get All Attachment for this RowId and Type
                var AllAttachments = (from tbl in _entities.FormAttachments
                                      where tbl.RefFormType == formtype && tbl.RefId == _recId orderby 
                                      tbl.UploadedDate descending
                                      select tbl).ToList();

                var AllActivity = (from tbl in _entities.Activities
                                   where tbl.RecordId == _recId
                                   orderby tbl.ActivityDate descending
                                   select new { tbl.ActivityDescription,ActivityDate=tbl.ActivityDate,tbl.ActivityStatus,tbl.ActivityByName }).ToList();


                cd.Comments = AllComments;
                cd.Attchments = AllAttachments;
                cd.Activity = AllActivity;
                return cd;         
            }
        }


        public dynamic GetPrintClaim(string Claim, char Type)
        {
            ClaimDetails cd = new ClaimDetails();

            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                Guid _recId = new Guid(Claim);
                Int16 formtype = 1;

                if (Type == 'g')
                {
                    //  var _generalclaim = _entities.FormGeneralLiabilityClaims.Where(x => x.GLLId == _recId).FirstOrDefault();

                    var _generalclaim = (from tbl in _entities.FormGeneralLiabilityClaims
                                         join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                         where tbl.GLLId == _recId
                                         select new { tbl }).FirstOrDefault();
                    if (_generalclaim != null)
                    {
                        return _generalclaim.tbl;
                    }                  
                }
                else if (Type == 'm')
                {
                    var _formdamageclaim = (from tbl in _entities.FormMoldDamageClaims
                                            join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                            where tbl.MDLId == _recId
                                            select new { tbl }).FirstOrDefault();

                    //_entities.FormMoldDamageClaims.Where(x => x.MDLId == _recId).FirstOrDefault();
                    if (_formdamageclaim != null)
                    {  return _formdamageclaim.tbl; }
                  
                }
                else if (Type == 'p')
                {
                    var _damageclaim = (from tbl in _entities.FormPropertyDamageClaims
                                        join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                        where tbl.PDLId == _recId
                                        select new { tbl }).FirstOrDefault();

                    //_entities.FormPropertyDamageClaims.Where(x => x.PDLId == _recId).FirstOrDefault();
                    if (_damageclaim != null)
                    {
                        return _damageclaim.tbl;
                    }
                   
                }

                return cd;
            }
        }

        public dynamic GetAllActivity(Guid _recId)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                var AllActivity = (from tbl in _entities.Activities
                                   where tbl.RecordId == _recId
                                   orderby tbl.ActivityDate descending
                                   select new { tbl.ActivityDescription, ActivityDate = tbl.ActivityDate, tbl.ActivityStatus, tbl.ActivityByName }).ToList();

                return AllActivity;
            }
        }

        public dynamic InsertComment(FormComment _property)
        {
            using (CarrollFormsEntities _entities= new CarrollFormsEntities())
            {
                           _property.CommentId = Guid.NewGuid();
                           _property.CommentDate = DateTime.Now;
                    // No record exists create a new property record here
                    _entities.FormComments.Add(_property);
                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();
                var AllComments = (from tbl in _entities.FormComments
                                   where tbl.RefFormID == _property.RefFormID && tbl.RefFormType == _property.RefFormType orderby
                                   tbl.CommentDate descending
                                   select tbl).ToList();
                string Comment = "A new comment was added by " + _property.CommentByName;
                LogActivity(Comment, _property.CommentByName, _property.CommentBy.ToString(), _property.RefFormID.ToString(), "New Comment");
                return new { comments = AllComments, activity = GetAllActivity(_property.RefFormID) } ;
            }
        }
       

        public dynamic InsertAttachment(FormAttachment formAttachment)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

               FormAttachment _property = formAttachment;               
                _property.AttachmentId = Guid.NewGuid();
                _property.UploadedDate = DateTime.Now;
                // No record exists create a new property record here
                _entities.FormAttachments.Add(_property);
                // _entities.SaveChanges();
                int i = _entities.SaveChanges();
                var AllAttachments = (from tbl in _entities.FormAttachments
                                   where tbl.RefId == formAttachment.RefId && tbl.RefFormType == _property.RefFormType
                                   orderby tbl.UploadedDate descending
                                   select tbl).ToList();
                string Comment = "A new attachement was added by " + _property.UploadedByName;
                LogActivity(Comment, _property.UploadedByName, _property.UploadedBy.ToString(), _property.RefId.ToString(), "New Attachment");
                return new { attachments = AllAttachments, activity = GetAllActivity(formAttachment.RefId) }; ;
            }

        }


        public void LogActivity(string ActivityDesc, string UserName, string UserGuid, string RecordId, string ActivityStatus)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    Activity _activity = new Activity();
                    _activity.ActivityId = System.Guid.NewGuid();
                    _activity.ActivityDescription = ActivityDesc;
                    _activity.ActivityDate = DateTime.Now;
                    _activity.ActivityBy = new Guid(UserGuid);
                    _activity.ActivityByName = UserName;
                    _activity.RecordId = new Guid(RecordId);
                    _activity.ActivityStatus = ActivityStatus;
                    _entities.Activities.Add(_activity);
                    _entities.SaveChanges();
                }
                catch(Exception ex)
                {

                }
             

            }

        }

        #region HR Forms


        public dynamic InsertEmployeeLeaseRider(EmployeeLeaseRaider formAttachment)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {                
                try
                {
                    EmployeeLeaseRaider _property = formAttachment;

                    // No record exists create a new property record here
                    _entities.EmployeeLeaseRaiders.Add(_property);
                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();
              
                    return new { Error = false, ErrorMsg = "", InsertedId = _property.EmployeeLeaseRiderId };
                }
                catch (Exception ex)
                {
                   return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }                                
            }
        }

        public dynamic GetEmployeeLeaseRider(Guid riderid)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var res = (from tbl in _entities.EmployeeLeaseRaiders
                               where tbl.EmployeeLeaseRiderId == riderid
                               select tbl).FirstOrDefault();

                    return res;
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }

        public dynamic InsertEmployeeNewHireNotice(EmployeeNewHireNotice formAttachment)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {
                    EmployeeNewHireNotice _property = formAttachment;

                    // No record exists create a new property record here
                    _entities.EmployeeNewHireNotices.Add(_property);
                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();

                    return new { Error = false, ErrorMsg = "", InsertedId = _property.EmployeeHireNoticeId };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }

        public dynamic GetEmployeeNewHireNotice(Guid riderid)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var res = (from tbl in _entities.EmployeeNewHireNotices
                               where tbl.EmployeeHireNoticeId == riderid
                               select tbl).FirstOrDefault();

                    return res;
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }



        public dynamic InsertPayRollStatusChangeNotice(PayrollStatusChangeNotice formAttachment)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {
                    PayrollStatusChangeNotice _property = formAttachment;

                    // No record exists create a new property record here
                    _entities.PayrollStatusChangeNotices.Add(_property);
                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();

                    return new { Error = false, ErrorMsg = "", InsertedId = _property.PayrollStatusChangeNoticeId };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }

        public dynamic GetPayRollStatusChangeNotice(Guid riderid)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var res = (from tbl in _entities.PayrollStatusChangeNotices
                               where tbl.PayrollStatusChangeNoticeId == riderid
                               select tbl).FirstOrDefault();

                    return res;
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }



        public dynamic InsertNoticeOfEmployeeSeperation(NoticeOfEmployeeSeperation formAttachment)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {
                    NoticeOfEmployeeSeperation _property = formAttachment;

                    // No record exists create a new property record here
                    _entities.NoticeOfEmployeeSeperations.Add(_property);
                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();

                    return new { Error = false, ErrorMsg = "", InsertedId = _property.EmployeeSeperationId };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }

        public dynamic GetNoticeOfEmployeeSeperation(Guid riderid)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var res = (from tbl in _entities.NoticeOfEmployeeSeperations
                               where tbl.EmployeeSeperationId == riderid
                               select tbl).FirstOrDefault();

                    return res;
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }



        public dynamic InsertRequistionRequest(RequisitionRequest formAttachment)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {
                    RequisitionRequest _property = formAttachment;

                    // No record exists create a new property record here
                    _entities.RequisitionRequests.Add(_property);
                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();

                    return new { Error = false, ErrorMsg = "", InsertedId = _property.RequisitionRequestId };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }


        public dynamic GetRequisitionRequest(Guid riderid)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var res = (from tbl in _entities.RequisitionRequests
                               where tbl.RequisitionRequestId == riderid
                               select tbl).FirstOrDefault();

                    return res;
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }





        public dynamic InsertMileageLog(MileageLogHeader mlh,List<MileageLogDetail> mld)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {
                    MileageLogHeader _property = mlh;

                    // No record exists create a new property record here
                    _entities.MileageLogHeaders.Add(_property);
                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();

                    foreach (var item in mld)
                    {
                        MileageLogDetail md = item;
                        _entities.MileageLogDetails.Add(md);

                    }
                    int i1 = _entities.SaveChanges();


                    return new { Error = false, ErrorMsg = "", InsertedId = _property.MonthlyMileageLogId };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }


        public dynamic GetMileageLog(Guid riderid)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var res = (from tbl in _entities.MileageLogHeaders
                               where tbl.MonthlyMileageLogId == riderid
                               select tbl).FirstOrDefault();

                    var res2 = (from tbl in _entities.MileageLogDetails
                                where tbl.MileageLogId == riderid
                                select tbl).ToList();


                    return new { header=res,details=res2 };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }

        public dynamic InsertExpenseReimbursement(ExpenseReimbursementHeader mlh, List<ExpenseReimbursementDetail> mld)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {
                    ExpenseReimbursementHeader _property = mlh;

                    // No record exists create a new property record here
                    _entities.ExpenseReimbursementHeaders.Add(_property);
                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();

                    foreach (var item in mld)
                    {
                        ExpenseReimbursementDetail md = item;
                        _entities.ExpenseReimbursementDetails.Add(md);
                    }

                    int i1 = _entities.SaveChanges();


                    return new { Error = false, ErrorMsg = "", InsertedId = _property.ExpenseReimbursementId };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }


        public dynamic GetExpenseReimbursement(Guid riderid)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var res = (from tbl in _entities.ExpenseReimbursementHeaders
                               where tbl.ExpenseReimbursementId == riderid
                               select tbl).FirstOrDefault();

                    var res2 = (from tbl in _entities.ExpenseReimbursementDetails
                                where tbl.ExpenseReimbursementId == riderid
                                select tbl).ToList();


                    return new { header = res, details = res2 };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }

        public dynamic GetHrFormCount()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {


                var ClaimCounts = new { LeaseCount = 0, PayrollCount = 0, EmployeeSeparationCount = 0,NewHireCount=0 };

                var leasecount = (from tbl in _entities.EmployeeLeaseRaiders
                                  select tbl).Count();
                var payrollcount= (from tbl in _entities.PayrollStatusChangeNotices
                                   select tbl).Count();
                var seperationcount = (from tbl in _entities.NoticeOfEmployeeSeperations
                                    select tbl).Count();
                var newhirecount = (from tbl in _entities.EmployeeNewHireNotices
                                    select tbl).Count();

                return new { LeaseCount = leasecount, PayRollCount = payrollcount, SeparationCount = seperationcount , HireCount=newhirecount};
            }
        }
        
        public dynamic GetAllMileageForms(string FormType,Guid userid, string optionalSeachText)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;

                var config = new Config { };

                if (FormType == "Mileage Log")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallexpensemileagelogs(userid).ToList();
                    else
                        config.Rows = _entities.proc_getallexpensemileagelogs(userid).Where(x => x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower()) || x.TotalPrice.Value.ToString().ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(MileageLogHeader).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Submitter Name", type = 0, href = "" });
                  //  config.Columns.Add(new DtableConfigArray { name = "reportedMonthMileage", label = "Month Mileage", type = DFieldType.IsDate, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "employeeSocialSecuirtyNumber", label = "Social SecuirtyNumber", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "emailAddress", label = "EmailAddress", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "manager", label = "Manager", type = 0, href = "" });
                 //   config.Columns.Add(new DtableConfigArray { name = "totalNumberOfMiles", label = "Total Miles", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "position_Exempt", label = "Position_Exempt", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "position_NonExempt", label = "Position_NonExempt", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "totalPrice", label = "Total Price", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "status", label = "Status", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "wage_Salary", label = "Wage_Salary", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "allocation", label = "Allocation", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "createdDatetime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }
                else if (FormType == "Expense Reimbursement")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallmonthlyexpensedetails(userid).ToList();
                    else
                        config.Rows = _entities.proc_getallmonthlyexpensedetails(userid).Where(x => x.Name.ToLower().Contains(optionalSeachText.ToLower()) || x.TotalExpenses.Value.ToString().ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(ExpenseReimbursementHeader).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "name", label = "Submitter Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "submittionDate", label = "Submission Date", type = DFieldType.IsDate, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "employeeSocialSecuirtyNumber", label = "Social SecuirtyNumber", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "emailAddress", label = "EmailAddress", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "manager", label = "Manager", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "totalExpenses", label = "Total Expenses", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "position_Exempt", label = "Position_Exempt", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "position_NonExempt", label = "Position_NonExempt", type = 0, href = "" });
                 //   config.Columns.Add(new DtableConfigArray { name = "balanceDue", label = "Balance Due", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "status", label = "Status", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "wage_Salary", label = "Wage_Salary", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "allocation", label = "Allocation", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                 //   config.Columns.Add(new DtableConfigArray { name = "createdDatetime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }


                return config;

            }


            }


            public dynamic GetAllHrForms(string FormType, string optionalSeachText)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;

                var config = new Config { };

                if(FormType == "LeaseRider")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallemployeeleaseriders().ToList();
                    else
                        config.Rows = _entities.proc_getallemployeeleaseriders().Where(x => x.Community.ToLower().Contains(optionalSeachText.ToLower()) || x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower()) || x.Position.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(EmployeeLeaseRaider).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "community", label = "Property", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Employee", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "date", label = "Date", type = DFieldType.IsDate, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "apartmentMarketRentalValue", label = "Rental Value", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "employeeMonthlyRent", label = "Monthly Rent", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "rentalPaymentResidencyAt", label = "Rental Payment At", type = 0, href = "" });                   
                    //config.Columns.Add(new DtableConfigArray { name = "position", label = "Position", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "createdDatetime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });



                }

               else if (FormType == "Requisition Request")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallrequisitionrequests().ToList();
                    else
                        config.Rows = _entities.proc_getallrequisitionrequests().Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.RequestorName.ToLower().Contains(optionalSeachText.ToLower()) || x.RequestorPosition.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(RequisitionRequest).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "propertyName", label = "Property Name" , type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "requestorName", label = "Requestor Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "requestorPosition", label = "Position", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "type", label = "Type", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "post", label = "Post", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "createdDateTime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });



                }
                else if(FormType=="PayRollChange")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallpayrollstatuschange().ToList();
                    else
                        config.Rows = _entities.proc_getallpayrollstatuschange().Where(x => x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower()) || x.ChangeEffectiveDate.Value.ToShortDateString().ToLower().Contains(optionalSeachText.ToLower()) || x.TypeOfChange.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(PayrollStatusChangeNotice).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Employee Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "changeEffectiveDate", label = "Effective Date", type = DFieldType.IsDate, href = "" });
                   config.Columns.Add(new DtableConfigArray { name = "typeOfChange", label = "Type Of Change", type = 0, href = "" });
                   //config.Columns.Add(new DtableConfigArray { name = "todayDate", label = "Today Date", type = DFieldType.IsDate, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "client_Location", label = "Client Location", type = 0, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "newHire", label = "NewHire", type = 0, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "reHire", label = "ReHire", type = 0, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "transfer", label = "Transfer", type = 0, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "position", label = "Position", type = 0, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "exempt", label = "Exempt", type =0, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "ssHash", label = "SS", type =0, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "dob", label = "DOB", type = DFieldType.IsDate, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "status_FullTime_PartTime", label = "Status", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "wage_Salary", label = "Wage/Salary", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "allocation", label = "Allocation", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "streetAddress", label = "Street Address", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "city_State_Zip", label = "City/State/Zip", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "phone", label = "Phone", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "change_Pay_Rate_From", label = "Change Pay Rate From", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "change_Pay_Rate_To", label = "Change Pay Rate To", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "change_Property_From", label = "Change Property From", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "change_Property_To", label = "Change Property To", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "address_ContactInfo", label = "Address ContactInfo", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "date_Of_Suspence", label = "Date Of Suspence", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "suspence_Paid", label = "Suspence Paid", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "suspence_UnPaid", label = "Suspence UnPaid", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "leave_Absence", label = "Leave Absence", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "leave_Paid", label = "Leave Paid", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "leave_UnPaid", label = "Leave UnPaid", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "explanation", label = "Explanation", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "createdDateTime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }
                //else if (FormType == "RequisitionRequest")
                //{
                //    if (string.IsNullOrEmpty(optionalSeachText))
                //        config.Rows = _entities.proc_getallrequisitionrequests().ToList();
                //    else
                //        config.Rows = _entities.proc_getallrequisitionrequests().Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.DatePosted.Value.ToShortDateString().ToLower().Contains(optionalSeachText.ToLower()) || x.RequestorName.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                //    config.EtType = EntityType.AllClaims.ToString();
                //    PropertyInfo[] userprop = typeof(PayrollStatusChangeNotice).GetProperties();
                //    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                //    config.Columns = new List<DtableConfigArray>();

                //    config.Columns.Add(new DtableConfigArray { name = "propertyName", label = "Property Name", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "requestorName", label = "Requestor Name", type = DFieldType.IsDate, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "requstorPosition", label = "Position", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "type", label = "Type", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "post", label = "Post", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "createdDateTime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                //}
                else if (FormType == "EmployeeSeparation")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallnoticeofemployeeseparation().ToList();
                    else
                        config.Rows = _entities.proc_getallnoticeofemployeeseparation().Where(x => x.EffectiveDateOfChange.Value.ToString().ToLower().Contains(optionalSeachText.ToLower()) || x.EligibleForReHire.ToString().ToLower().Contains(optionalSeachText.ToLower()) || x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.PropertyNumber.ToLower().Contains(optionalSeachText.ToLower()) || x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower()) || x.JobTitle.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(NoticeOfEmployeeSeperation).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    //config.Columns.Add(new DtableConfigArray { name = "effectiveDateOfChange", label = "EffectiveDateOfChange", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "eligibleForReHire", label = "EligibleForReHire", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "propertyName", label = "Property Name", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "propertyNumber", label = "PropertyNumber", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Employee Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "jobTitle", label = "Employee Position", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "policty_Voilated", label = "Policty_Voilated", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "reason", label = "Reason", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "documentationAvailable", label = "DocumentationAvailable", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "warningGiven_Dates", label = "WarningGiven_Dates", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "equipmentKeysReturned", label = "EquipmentKeysReturned", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "c2WeeeksNoticeGiven", label = "C2WeeeksNoticeGiven", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "vacationPaidOut", label = "VacationPaidOut", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "vacationBalance", label = "VacationBalance", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "notes_Comments", label = "Notes_Comments", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "createdDateTime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }
                else if (FormType == "New Hire Notice")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallemployeenewhirenotice().ToList();
                    else
                        config.Rows = _entities.proc_getallemployeenewhirenotice().Where(x => x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower()) || x.StartDate.Value.ToShortDateString().ToString().ToLower().Contains(optionalSeachText.ToLower()) || x.EmployeeSocialSecuirtyNumber.ToLower().Contains(optionalSeachText.ToLower()) || x.EmailAddress.ToLower().Contains(optionalSeachText.ToLower()) || x.Manager.ToLower().Contains(optionalSeachText.ToLower()) || x.Location.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(EmployeeNewHireNotice).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Employee Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "startDate", label = "Start Date", type = DFieldType.IsDate, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "employeeSocialSecuirtyNumber", label = "Social SecuirtyNumber", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "emailAddress", label = "EmailAddress", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "manager", label = "Manager", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "location", label = "Location", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "position_Exempt", label = "Position_Exempt", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "position_NonExempt", label = "Position_NonExempt", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "position", label = "Position", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "status", label = "Status", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "wage_Salary", label = "Wage_Salary", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "allocation", label = "Allocation", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "createdDateTime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }
                //else if (FormType == "Mileage Log")
                //{
                //    if (string.IsNullOrEmpty(optionalSeachText))
                //        config.Rows = _entities.proc_getallexpensemileagelogs().ToList();
                //    else
                //        config.Rows = _entities.proc_getallexpensemileagelogs().Where(x => x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower()) || x.TotalPrice.Value.ToString().ToLower().Contains(optionalSeachText.ToLower())).ToList();

                //    config.EtType = EntityType.AllClaims.ToString();
                //    PropertyInfo[] userprop = typeof(MileageLogHeader).GetProperties();
                //    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                //    config.Columns = new List<DtableConfigArray>();

                //    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Employee Name", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "reportedMonthMileage", label = "Month Mileage", type = DFieldType.IsDate, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "employeeSocialSecuirtyNumber", label = "Social SecuirtyNumber", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "emailAddress", label = "EmailAddress", type = 0, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "manager", label = "Manager", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "totalNumberOfMiles", label = "Total Miles", type = 0, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "position_Exempt", label = "Position_Exempt", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "position_NonExempt", label = "Position_NonExempt", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "totalPrice", label = "Total Price", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "status", label = "Status", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "wage_Salary", label = "Wage_Salary", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "allocation", label = "Allocation", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "createdDatetime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                //}
                //else if (FormType == "Expense Reimbursement")
                //{
                //    if (string.IsNullOrEmpty(optionalSeachText))
                //        config.Rows = _entities.proc_getallmonthlyexpensedetails().ToList();
                //    else
                //        config.Rows = _entities.proc_getallmonthlyexpensedetails().Where(x => x.Name.ToLower().Contains(optionalSeachText.ToLower()) || x.TotalExpenses.Value.ToString().ToLower().Contains(optionalSeachText.ToLower())).ToList();

                //    config.EtType = EntityType.AllClaims.ToString();
                //    PropertyInfo[] userprop = typeof(ExpenseReimbursementHeader).GetProperties();
                //    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                //    config.Columns = new List<DtableConfigArray>();

                //    config.Columns.Add(new DtableConfigArray { name = "name", label = "Name", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "submissionDate", label = "Submission Date", type = DFieldType.IsDate, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "employeeSocialSecuirtyNumber", label = "Social SecuirtyNumber", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "emailAddress", label = "EmailAddress", type = 0, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "manager", label = "Manager", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "totalExpenses", label = "Total Expenses", type = 0, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "position_Exempt", label = "Position_Exempt", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "position_NonExempt", label = "Position_NonExempt", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "balanceDue", label = "Balance Due", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "status", label = "Status", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "wage_Salary", label = "Wage_Salary", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "allocation", label = "Allocation", type = DFieldType.IsText, href = "" });
                //    //config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "createdDatetime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                //}


                return config;

            }

        }



        #endregion






        /// <summary>
        /// 
        /// Send Email alerts to Corresponding Receipients based on the Claim Type and Work Flow
        /// 0. Get the Claim row
        /// 1. Subject Compose
        /// 2. Body compose based on Claim Type, get all fields all showing in View Claim Page, better call that method to get those row and Form tr's for each row.
        /// 3. Get Mail Settings such as Host, Email, Password and Port settings
        /// 4. Get All Receipents with email, those can be used to update Activity
        /// 5. After sending email , update an acticity record for each user alert with proper description
        /// 
        /// </summary>
        /// <param name="recordid"></param>
        /// <param name="Form"></param>
        /// <returns></returns>

        //private void SendEmailAlert(string recordid, char Type)
        //{
        //    var emailsettings = new EmailParams();

        //    // var ClaimData = GetClaimDetails(recordid, Type);

        //    bool mailsent = false;
        //    string mailmsg = "";
        //    string attachmsg = "";

        //    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        //    string toemail = "";

        //    toemail = "sekhar.babu@forcitude.com";
        //    var _entities = new CarrollFormsEntities();
        //    var propid = new Guid(recordid);

        //    Guid propertyid = Guid.NewGuid();

        //    using (MailMessage mail = new MailMessage(new MailAddress(emailsettings.fromemail, emailsettings.Company), new MailAddress(emailsettings.fromemail, emailsettings.Company)))
        //    {

        //        if (Type == 'p')
        //        {

        //            var ClaimData = (from tbl in _entities.FormPropertyDamageClaims
        //                             join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
        //                             where tbl.PDLId == propid
        //                             select new { tbl, tblprop.PropertyName }).FirstOrDefault();
        //            propertyid = ClaimData.tbl.PropertyId;

        //            mail.Subject = "Alert New Property Damage Claim for " + ClaimData.PropertyName + " by " + ClaimData.tbl.IncidentReportedBy;
        //            emailsettings.mailbody = "";

        //            emailsettings.mailbody += "<tr><td> <strong> Incident Location : </strong> </td><td>" + (ClaimData.tbl.IncidentLocation == null ? "" : ClaimData.tbl.IncidentLocation) + "</td> <td><strong> Incident Date : </strong> </td><td>" + ClaimData.tbl.IncidentDateTime.ToShortDateString() + "</td> </tr>";

        //            emailsettings.mailbody += "<tr><td><strong> Weather Conditions : </strong> </td> <td>" + (ClaimData.tbl.WeatherConditions == null ? "" : ClaimData.tbl.WeatherConditions) + " </td><td><strong> Incident Description : </strong> </td><td>" + ClaimData.tbl.IncidentDescription + "</td></tr>";
        //            if (ClaimData.tbl.AuthoritiesContacted == false)
        //                emailsettings.mailbody += "<tr><td><strong> Estimate Of Damage : </strong> </td><td> " + (ClaimData.tbl.EstimateOfDamage == null ? "" : ClaimData.tbl.EstimateOfDamage) + "</td> <td><strong> Authorities Contacted : </strong> </td><td> No </td></tr>";
        //            else
        //                emailsettings.mailbody += "<tr><td><strong> Estimate Of Damage : </strong> </td><td> " + (ClaimData.tbl.EstimateOfDamage == null ? "" : ClaimData.tbl.EstimateOfDamage) + "</td> <td><strong> Authorities Contacted : </strong> </td><td> Yes </td></tr>";
        //            if (ClaimData.tbl.LossOfRevenues == false)
        //                emailsettings.mailbody += "<tr><td><strong> Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> <td><strong> Loss Of Revenues : </strong> </td><td > No </td></tr>";
        //            else
        //                emailsettings.mailbody += "<tr><td><strong> Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> <td><strong> Loss Of Revenues : </strong> </td><td > Yes </td></tr>";


        //            if (ClaimData.tbl.WitnessPresent == false)
        //                emailsettings.mailbody += "<tr><td><strong> Witness Present : </strong> </td><td> No  </td> <td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr> ";
        //            else
        //                emailsettings.mailbody += "<tr><td>><strong> Witness Present : </strong> </td><td> Yes  </td> <td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr> ";

        //            emailsettings.mailbody += "<tr><td><strong> Witness Phone : </strong> </td><td> " + (ClaimData.tbl.WitnessPhone == null ? "" : ClaimData.tbl.WitnessPhone) + "</td> <td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td> </tr>";
        //            emailsettings.mailbody += "<tr> <td><strong> Witness Address : </strong> </td><td>" + ClaimData.tbl.WitnessAddress + "</td> <td><strong> Reported By : </strong> </td><td>" + ClaimData.tbl.IncidentReportedBy + "</td></tr>";

        //            emailsettings.mailbody += "<tr><td><strong> Reported Phone : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";

        //            emailsettings.mailbody += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate + "</td></tr>";


        //            emailsettings.mailbody += "<tr><td><strong> Incident Date : </strong> </td><td>" + ClaimData.tbl.IncidentDateTime.ToShortDateString() + "</td></tr>";

        //            emailsettings.mailbody += "<tr></tr>";

        //            if (ClaimData.tbl.AuthoritiesContacted == false)
        //                emailsettings.mailbody += "<tr><td><strong> Authorities Contacted : </strong> </td><td> No </td></tr>";
        //            else
        //                emailsettings.mailbody += "<tr><td><strong> Authorities Contacted : </strong> </td><td> Yes </td></tr>";
        //            if (ClaimData.tbl.LossOfRevenues == false)
        //                emailsettings.mailbody += "<tr><td><strong> Loss Of Revenues : </strong> </td><td > No </td></tr>";
        //            else
        //                emailsettings.mailbody += "<tr><td><strong> Loss Of Revenues : </strong> </td><td> Yes </td></tr>";

        //            emailsettings.mailbody += "<tr></tr>";
        //            emailsettings.mailbody += "<tr></tr>";
        //            emailsettings.mailbody += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToShortDateString()) + "</td></tr>";
        //            emailsettings.mailbody += "<tr><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr></table>";

        //        }
        //        else if (Type == 'm')
        //        {

        //            var ClaimData = (from tbl in _entities.FormMoldDamageClaims
        //                             join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
        //                             where tbl.MDLId == propid
        //                             select new { tbl, tblprop.PropertyName }).FirstOrDefault();
        //            propertyid = ClaimData.tbl.PropertyId;

        //            mail.Subject = "Alert New Mold Damage Claim for " + ClaimData.PropertyName + " by " + ClaimData.tbl.ReportedBy;

        //        }
        //        else if (Type == 'g')
        //        {
        //            var ClaimData = (from tbl in _entities.FormGeneralLiabilityClaims
        //                             join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
        //                             where tbl.GLLId == propid
        //                             select new { tbl, tblprop.PropertyName }).FirstOrDefault();

        //            propertyid = ClaimData.tbl.PropertyId;
        //            mail.Subject = "Alert New General Liability Claim for " + ClaimData.PropertyName + " by " + ClaimData.tbl.ReportedBy;

        //        }






        //        //string path =System.Web.HttpContext.Current.Server.MapPath(@"img/" + emailsettings.logo);
        //        //LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
        //        //Img.ContentId = "MyImage";

        //        //   string header = emailsettings.mailstart + "<p> Dear Customer " + item.ContactName + "  </p>  <br> ";


        //        //     outstandingmodel.cms = outstandingmodel.cms.Replace("[Customer Name]", item.CustomerName + "<br>");

        //        //  mail.Body = header + outstandingmodel.cms + mailfooterhtml;
        //        //now do the HTML formatting
        //        AlternateView av1 = AlternateView.CreateAlternateViewFromString(
        //            emailsettings.mailstart + emailsettings.mailbody + emailsettings.signature + emailsettings.mailfooterhtml,
        //              null, MediaTypeNames.Text.Html);

        //        //now add the AlternateView
        //        // av1.LinkedResources.Add(Img);

        //        //now append it to the body of the mail
        //        mail.AlternateViews.Add(av1);

        //        mail.IsBodyHtml = true;

        //        #region Attachments Adding
        //        //var actionPDF = new Rotativa.ViewAsPdf("PrintInvoice", d)//some route values)
        //        //{
        //        //    //FileName = "TestView.pdf",
        //        //    PageSize = Size.A4,
        //        //    PageOrientation = Rotativa.Options.Orientation.Portrait,
        //        //    PageMargins = { Left = 1, Right = 1 }
        //        //};


        //        //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

        //        //MemoryStream file = new MemoryStream(applicationPDFData);
        //        //file.Seek(0, SeekOrigin.Begin);

        //        //Attachment data = new Attachment(file, item.invoiceclosenumber + " - Invoice Details.pdf", "application/pdf");
        //        //attachmsg = "";
        //        //attachmsg += data.Name;
        //        //ContentDisposition disposition = data.ContentDisposition;
        //        //disposition.CreationDate = System.DateTime.Now;
        //        //disposition.ModificationDate = System.DateTime.Now;
        //        //disposition.DispositionType = DispositionTypeNames.Attachment;

        //        //mail.Attachments.Add(data);
        //        //if (outstandingmodel.includethmb)
        //        //    if (attachments != null)
        //        //    {
        //        //        foreach (HttpPostedFileBase attachment in attachments)
        //        //        {
        //        //            if (attachment != null)
        //        //            {
        //        //                string fileName = Path.GetFileName(attachment.FileName);
        //        //                attachmsg += string.IsNullOrEmpty(attachmsg) ? fileName : "," + fileName;

        //        //                mail.Attachments.Add(new Attachment(attachment.InputStream, fileName));
        //        //            }
        //        //        }

        //        //    }
        //        #endregion

        //        SmtpClient smtp = SetMailServerSettings();

        //        // get all emails to send and attach them one by one ( based on property )


        //        var proprow = (from tbl in _entities.Properties
        //                       where tbl.PropertyId == propertyid
        //                       select new { tbl.EmailAddress, tbl.InsuranceNotifyEmail }).FirstOrDefault();


        //        mailsent = false;
        //        mailmsg = "";

        //        bool validemail = false;
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(proprow.EmailAddress))
        //            {

        //                Match match = regex.Match(proprow.EmailAddress);
        //                if (match.Success)
        //                {
        //                    mail.To.Add(proprow.EmailAddress);
        //                    validemail = true;
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(proprow.InsuranceNotifyEmail))
        //            {
        //                Match match = regex.Match(proprow.InsuranceNotifyEmail);
        //                if (match.Success)
        //                {
        //                    mail.To.Add(proprow.InsuranceNotifyEmail);
        //                    validemail = true;
        //                }
        //            }

        //            if (validemail)
        //            {
        //                mail.To.Clear();
        //                mail.To.Add(toemail);
        //                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        //                mail.Priority = MailPriority.High;

        //                smtp.Send(mail);
        //                mailsent = true;

        //                // Activity  Insertion Goes Here


        //            }
        //            //else
        //            //    error.ErrorList.Add(new Error { ErrorExist = true, ErrorType = "Invalid Email Address", ErrorMsg = " Invalid Email For Customer " + item.CustomerName + " : Invoice Number " + item.InvoiceNumber });

        //        }
        //        catch (Exception ex)
        //        {
        //            //mailsent = false;
        //            //mailmsg = ex.Message;

        //            //error.ErrorList.Add(new Error { ErrorExist = true, ErrorType = " Mail Function Failure", ErrorMsg = ex.Message });

        //        }

        //    }

        //}

        public SmtpClient SetMailServerSettings()
        {

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            NetworkCredential networkCredential = new NetworkCredential("sekhar.babu@forcitude.com", "R21221.Skr");

            smtp.Credentials = networkCredential;
            smtp.Port = 587; //587
            return smtp;

        }

        public dynamic GetUserPropertyForClaimPrint(string userid)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                PrintProperty pp = new PrintProperty();

                _entities.Configuration.ProxyCreationEnabled = false;
                Guid _propId = new Guid(userid);

                var propertyres = _entities.proc_getpropertydetails(_propId).FirstOrDefault();

                if (propertyres != null)
                    return propertyres;
                else
                    return null;
            }
        }

        public List<CarrollPayPeriod> GetAllCarrollPayPerilds()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                                             
                //us
                var propertyres =  _entities.CarrollPayPeriods.ToList();



                if (propertyres != null)
                    return propertyres;
                else
                    return null;
            }
        }

        public string GetPropertyName(int PropertyNumber)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {

                //us
                var propertyres = (from tbl in _entities.Properties
                                   where tbl.PropertyNumber == PropertyNumber
                                   select tbl.PropertyName).FirstOrDefault();

                if (propertyres != null)
                    return propertyres;
                else
                    return "";
            }
        }

        public string GetPropertyNameManager(int PropertyNumber)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {

                //us
                var propertyres = (from tbl in _entities.Properties
                                   where tbl.PropertyNumber == PropertyNumber
                                   select new { tbl.PropertyName,tbl.PropertyManager }).FirstOrDefault();
                var manager = "";
                if(propertyres.PropertyManager != null)
                {

                    var mananger1 = (from tbl in _entities.Contacts
                                   where tbl.ContactId == propertyres.PropertyManager
                                   select new { tbl.FirstName, tbl.LastName, tbl.Email }).FirstOrDefault();
                    manager = mananger1.FirstName + " " + mananger1.LastName;

                }

                if (propertyres != null)
                    return propertyres.PropertyName+","+manager;
                else
                    return "";
            }
        }

        public List<CarrollPosition> GetAllCarrollPositions()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                var propertyres =  _entities.CarrollPositions.ToList();

                if (propertyres != null)
                    return propertyres;
                else
                    return null;

            }
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
