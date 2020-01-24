using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
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
    public class EntityDataRepository : IDataRepository
    {

        public CarrollFormsEntities DBEntity => new EFConnectionAccessor().Entities;

        /// <summary>
        /// Generic delete
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        /// 
        public bool DeleteRecord(EntityType entityType, string recordId)
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


        public dynamic GetRecord(EntityType entityType, string recordId)
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
                        var _damageclaim = _entities.FormPropertyDamageClaims.Where(x => x.PDLId == _recId).FirstOrDefault();
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

        public dynamic CreateUpdateRecord(EntityType entityType, dynamic obj)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                RecordUpdateResult _result = new RecordUpdateResult();
                var username = "";

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
                                username = _property.CreatedByName;
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
                                username = _property.CreatedByName;
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
                                username = _contact.CreatedByName;
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
                                username = _contact.CreatedByName;
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
                                username = _partner.CreatedByName;
                                return _result;
                            }
                            else
                            {

                                _entities.Entry(_dbpartner).CurrentValues.SetValues(_partner);
                                int i = _entities.SaveChanges();
                                // return true;
                                _result.RecordId = _partner.EquityPartnerId.ToString();
                                _result.Succeded = true;
                                username = _partner.CreatedByName;
                                return _result;

                            }
                        #endregion

                        case EntityType.User:
                            #region [ User ] 

                            SiteUser _user = obj;
                            var _dbuser = _entities.SiteUsers.Where(x => x.UserId == _user.UserId).FirstOrDefault();
                            if (_dbuser == null)
                            {

                                var _dbuser1 = _entities.SiteUsers.Where(x => x.UserEmail == _user.UserEmail).FirstOrDefault();

                                if(_dbuser1 != null)
                                {
                                    _result.RecordId = "user already exists";
                                    _result.Succeded = false;

                                    return _result;
                                }

                                if ((_user.UserId.ToString() == "00000000-0000-0000-0000-000000000000") || (_user.UserId == null))
                                {
                                    _user.UserId = Guid.NewGuid();
                                }
                                _user.CreatedDate = DateTime.Now;
                                // No record exists create a new property record here
                                username = _user.CreatedByName;
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
                                username = _user.CreatedByName;
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

                                var res = (from tbl in _entities.UserInRoles
                                           join tb in _entities.SiteUsers  on tbl.UserId equals tb.UserId
                                           join tblrole in _entities.Roles on tbl.RoleId equals tblrole.RoleId
                                           where tbl.UserId == _userrole.UserId
                                           select new { tblrole.RoleName,tb.UserEmail }).FirstOrDefault();

                                if (res != null)
                                {
                                    _result.RecordId = res.UserEmail+" is already assigned to "+res.RoleName;
                                    _result.Succeded = false;

                                    return _result;
                                }


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

                                string Comment = "FormGeneralLiabilityClaim Record was added on " + Convert.ToDateTime(_glc.CreatedDate).ToString("MM/dd/yyyy");
                                LogActivity(Comment, _glc.CreatedByName, _glc.CreatedBy.ToString(), _glc.GLLId.ToString(), "New GL Claim");
                                // return (i == 1) ? true : false;
                                // return true;
                                _result.RecordId = _glc.GLLId.ToString();
                                _result.Succeded = true;
                                username = _glc.CreatedByName;
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
                                username = _glc.CreatedByName;
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
                                string Comment = "Mold Damage Claim Record was added on " + Convert.ToDateTime(_mdc.CreatedDate).ToString("MM/dd/yyyy");
                                LogActivity(Comment, _mdc.CreatedByName, _mdc.CreatedBy.ToString(), _mdc.MDLId.ToString(), "New MD Claim");
                                // return (i == 1) ? true : false;
                                // return true;
                                _result.RecordId = _mdc.MDLId.ToString();
                                _result.Succeded = true;
                                username = _mdc.CreatedByName;
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
                                username = _mdc.CreatedByName;
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


                                string Comment = "Property Damage Claim Record was added on " + Convert.ToDateTime(_pdc.CreatedDate).ToString("MM/dd/yyyy");
                                LogActivity(Comment, _pdc.CreatedByName, _pdc.CreatedBy.ToString(), _pdc.PDLId.ToString(), "New PD Claim");
                                // return (i == 1) ? true : false;
                                //return true;
                                _result.RecordId = _pdc.PDLId.ToString();
                                _result.Succeded = true;
                                username = _pdc.CreatedByName;
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
                                username = _pdc.CreatedByName;
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
                    StringBuilder sb = new StringBuilder();
                    Exception e = ex;

                    if (e.GetType() == typeof(DbEntityValidationException))
                    {
                        var innerException = e as DbEntityValidationException;
                        if (innerException != null)
                        {
                            sb.AppendLine();
                            sb.AppendLine();
                            foreach (var eve in innerException.EntityValidationErrors)
                            {
                                sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                        ve.PropertyName,
                                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                        ve.ErrorMessage));
                                }
                            }
                            sb.AppendLine();
                        }
                    }
                    else
                    {
                        if (e.InnerException != null)
                        {
                            sb.AppendLine();
                            sb.AppendLine();

                            if (e.InnerException != null)
                            {
                                sb.Append(e.InnerException.Message);
                            }

                          
                            sb.AppendLine();
                        }
                    }
                    //    Exception e = filterContext.Exception;
                    //Get a StackTrace object for the exception
                    StackTrace st = new StackTrace(e, true);

                    //Get the first stack frame
                    StackFrame frame = st.GetFrame(0);

                    //Get the file name
                    string fileName = frame.GetFileName();

                    //Get the method name
                    string methodName = frame.GetMethod().Name;

                    //Get the line number from the stack frame
                    int line = frame.GetFileLineNumber();

                    //Get the column number
                    int col = frame.GetFileColumnNumber();

                    ErrorLog errorLog = new ErrorLog();
                    errorLog.datetime = DateTime.Now;
                    errorLog.UserName = username;
                    errorLog.Page = "Form Submission "+entityType.ToString();
                    errorLog.Error = e.GetType().ToString() + " at" + "Form Submission " + entityType.ToString();
                    errorLog.Description = sb.ToString() + " " + e.Message + line.ToString();
                    errorLog.LogId = Guid.NewGuid();
                    ErrorLog(errorLog);
                    _result.RecordId = "";
                    _result.Succeded = false;
                    return _result;
                }

                return true;
            }

        }

       public dynamic GetAllContactsHighRolesInclude(string search)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                dynamic _temp = _entities.proc_getallcontactsincludinghighroles().ToList().Where(s => s.FirstName.ToLower().Contains(search) || s.LastName.ToLower().Contains(search)).ToList();
                return _temp;
            }
        }


        public dynamic GetRecords(EntityType entityType, string optionalSeachText = "")
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

                        List<Contact> _result = rep.GetAll().ToList();
                        if (!string.IsNullOrEmpty(optionalSeachText))
                        {
                            dynamic _temp = _result.Where(s => s.FirstName.ToLower().Contains(optionalSeachText) || s.LastName.ToLower().Contains(optionalSeachText)).ToList();
                            return _temp;
                        }
                        return _result;
                    //if (string.IsNullOrEmpty(optionalSeachText)) return _entities.Contacts.ToList();
                    //else return _entities.Contacts.Where(x => x.IsActive && (x.FirstName.Contains(optionalSeachText) || x.LastName.Contains(optionalSeachText) || x.Title.Contains(optionalSeachText) || x.Phone.Contains(optionalSeachText) || x.Email.Contains(optionalSeachText))).ToList();
                    #endregion

                    case EntityType.Partner:

                        #region [Partner]
                        List<proc_getequitypartners_Result> _result1 = _entities.proc_getequitypartners().ToList();
                        if (!string.IsNullOrEmpty(optionalSeachText))
                        {
                            dynamic _temp = _result1.Where(s => s.PartnerName.ToLower().Contains(optionalSeachText) || s.AddressLine1.ToLower().Contains(optionalSeachText) || s.AddressLine2.ToLower().Contains(optionalSeachText) || s.City.ToLower().Contains(optionalSeachText) || s.State.ToLower().Contains(optionalSeachText) || s.ZipCode.ToLower().Contains(optionalSeachText) || s.ContactPerson.ToLower().Contains(optionalSeachText)).ToList();
                            return _temp;
                        }
                        return _result1;

                        //if (string.IsNullOrEmpty(optionalSeachText))
                        //{
                        //    return (from tbl in _entities.EquityPartners
                        //            join tblcontact in _entities.Contacts on tbl.ContactId equals tblcontact.ContactId
                        //            select new { equityPartnerId = tbl.EquityPartnerId, partnerName = tbl.PartnerName, addressLine1 = tbl.AddressLine1, addressLine2 = tbl.AddressLine2, city = tbl.City, state = tbl.State, zipCode = tbl.ZipCode, contactId = tblcontact.FirstName + " " + tblcontact.LastName, createdDate = tbl.CreatedDate, createdByName = tbl.CreatedByName }).ToList();

                        //}
                        //else
                        //{
                        //    return (from tbl in _entities.EquityPartners
                        //            join tblcontact in _entities.Contacts on tbl.ContactId equals tblcontact.ContactId
                        //            where tbl.IsActive == true && (tbl.PartnerName.Contains(optionalSeachText) || tbl.AddressLine1.Contains(optionalSeachText) || tbl.AddressLine2.Contains(optionalSeachText) || tbl.City.Contains(optionalSeachText) || tbl.State.Contains(optionalSeachText))
                        //            select new { equityPartnerId = tbl.EquityPartnerId, partnerName = tbl.PartnerName, addressLine1 = tbl.AddressLine1, addressLine2 = tbl.AddressLine2, city = tbl.City, state = tbl.State, zipCode = tbl.ZipCode, contactId = tblcontact.FirstName + " " + tblcontact.LastName, createdDate = tbl.CreatedDate, createdByName = tbl.CreatedByName }).ToList();
                        //}


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
                        else return _entities.SiteUsers.Where(x => x.IsActive && (x.FirstName.Contains(optionalSeachText) || x.LastName.Contains(optionalSeachText) || x.Phone.Contains(optionalSeachText) || x.UserEmail.Contains(optionalSeachText))).ToList();
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
                           where tbluserrole.UserId == userid
                           select tbl).FirstOrDefault();

                if (res.RoleName.ToLower() == "administrator" || res.RoleName.ToLower() == "management")
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
                else if (res.RoleName.ToLower() == "property")
                 {

                    var _propcount = (from tbl in _entities.FormPropertyDamageClaims
                                      join tblpropertyusers in _entities.UserInProperties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                      where tblpropertyusers.UserId == userid
                                      select tbl).Count();
                    var _damagecount = (from tbl in _entities.FormMoldDamageClaims
                                        join tblpropertyusers in _entities.UserInProperties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                        where tblpropertyusers.UserId == userid
                                        select tbl).Count();
                    var _liabilitycount = (from tbl in _entities.FormGeneralLiabilityClaims
                                           join tblpropertyusers in _entities.UserInProperties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                           where tblpropertyusers.UserId == userid
                                           select tbl).Count();
                    return new { PropertyCount = _propcount, DamageCount = _damagecount, LiabilityCount = _liabilitycount };
                }
                else if (res.RoleName.ToLower() == "vp")
                {
                    var managementid = _entities.SiteUsers.Where(x => x.UserId == userid).Select(x => x.managementcontact).FirstOrDefault();

                    if (managementid != null)
                    {

                        var _propcount = (from tbl in _entities.FormPropertyDamageClaims
                                          join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                          where tblpropertyusers.VicePresident == managementid
                                          select tbl).Count();
                        var _damagecount = (from tbl in _entities.FormMoldDamageClaims
                                            join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                            where tblpropertyusers.VicePresident == managementid
                                            select tbl).Count();
                        var _liabilitycount = (from tbl in _entities.FormGeneralLiabilityClaims
                                               join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                               where tblpropertyusers.VicePresident == managementid
                                               select tbl).Count();
                        return new { PropertyCount = _propcount, DamageCount = _damagecount, LiabilityCount = _liabilitycount };
                    }
                    else
                        return new { PropertyCount = 0, DamageCount = 0, LiabilityCount = 0 };
                }
                else if (res.RoleName.ToLower() == "rvp")
                {
                    var managementid = _entities.SiteUsers.Where(x => x.UserId == userid).Select(x => x.managementcontact).FirstOrDefault();

                    if (managementid != null)
                    {

                        var _propcount = (from tbl in _entities.FormPropertyDamageClaims
                                          join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                          where tblpropertyusers.RegionalVicePresident == managementid
                                          select tbl).Count();
                        var _damagecount = (from tbl in _entities.FormMoldDamageClaims
                                            join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                            where tblpropertyusers.RegionalVicePresident == managementid
                                            select tbl).Count();
                        var _liabilitycount = (from tbl in _entities.FormGeneralLiabilityClaims
                                               join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                               where tblpropertyusers.RegionalVicePresident == managementid
                                               select tbl).Count();
                        return new { PropertyCount = _propcount, DamageCount = _damagecount, LiabilityCount = _liabilitycount };
                    }
                    else
                        return new { PropertyCount = 0, DamageCount = 0, LiabilityCount = 0 };
                }
                else if (res.RoleName.ToLower() == "regional")
                {
                    var managementid = _entities.SiteUsers.Where(x => x.UserId == userid).Select(x => x.managementcontact).FirstOrDefault();

                    if (managementid != null)
                    {
                        var _propcount = (from tbl in _entities.FormPropertyDamageClaims
                                          join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                          where tblpropertyusers.RegionalManager == managementid
                                          select tbl).Count();
                        var _damagecount = (from tbl in _entities.FormMoldDamageClaims
                                            join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                            where tblpropertyusers.RegionalManager == managementid
                                            select tbl).Count();
                        var _liabilitycount = (from tbl in _entities.FormGeneralLiabilityClaims
                                               join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                               where tblpropertyusers.RegionalManager == managementid
                                               select tbl).Count();
                        return new { PropertyCount = _propcount, DamageCount = _damagecount, LiabilityCount = _liabilitycount };
                    }
                    else
                        return new { PropertyCount = 0, DamageCount = 0, LiabilityCount = 0 };
                }
                else if (res.RoleName.ToLower() == "asset manager")
                {
                    var managementid = _entities.SiteUsers.Where(x => x.UserId == userid).Select(x => x.managementcontact).FirstOrDefault();

                    if (managementid != null)
                    {

                        var _propcount = (from tbl in _entities.FormPropertyDamageClaims
                                          join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                          where tblpropertyusers.AssetManager1 == managementid
                                          select tbl).Count();
                        var _damagecount = (from tbl in _entities.FormMoldDamageClaims
                                            join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                            where tblpropertyusers.AssetManager1 == managementid
                                            select tbl).Count();
                        var _liabilitycount = (from tbl in _entities.FormGeneralLiabilityClaims
                                               join tblpropertyusers in _entities.Properties on tbl.PropertyId equals tblpropertyusers.PropertyId
                                               where tblpropertyusers.AssetManager1 == managementid
                                               select tbl).Count();
                        return new { PropertyCount = _propcount, DamageCount = _damagecount, LiabilityCount = _liabilitycount };
                    }
                    else
                        return new { PropertyCount = 0, DamageCount = 0, LiabilityCount = 0 };
                }
                else
                {
                    return new { PropertyCount = 0, DamageCount = 0, LiabilityCount = 0 };
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
                var str = "";

                foreach (var item in res)
                {
                    if (str != "")
                        str += item.ToString();
                    else
                        str += "," + item.ToString();
                }

                return res;
            }
        }

        public dynamic GetEquityPartners()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {

                var res = (from tbl in _entities.EquityPartners                          
                           where tbl.IsActive == true
                           select tbl).ToList();

                return res;
            }
        }



        public dynamic GetAllClaims(Guid? userid, Guid? propertyid, string Type, string optionalSeachText,int orderby)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;

                var config = new Config { };
                if (orderby == 1)
                {
                    if (string.IsNullOrEmpty(optionalSeachText) && string.IsNullOrEmpty(Type))
                    config.Rows = _entities.SP_GetAllClaimsUpdatedDate(userid, propertyid).ToList().OrderByDescending(o=>o.Updateddate);
                else if (Type == "All" && string.IsNullOrEmpty(optionalSeachText))
                    config.Rows = _entities.SP_GetAllClaimsUpdatedDate(userid, propertyid).Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentLocation.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentDescription.ToLower().Contains(optionalSeachText.ToLower()) || x.ReportedBy.ToLower().Contains(optionalSeachText.ToLower())).ToList().OrderByDescending(o => o.Updateddate);
                else if (Type != "All" && string.IsNullOrEmpty(optionalSeachText))
                    config.Rows = _entities.SP_GetAllClaimsUpdatedDate(userid, propertyid).Where(x => (x.ClaimType.ToLower().Trim().Contains(Type.ToLower().Trim()))).ToList().OrderByDescending(o => o.Updateddate);
                else
                    config.Rows = _entities.SP_GetAllClaimsUpdatedDate(userid, propertyid).Where(x => (x.ClaimType.ToLower() == Type.ToLower() && (x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentLocation.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentDescription.ToLower().Contains(optionalSeachText.ToLower()) || x.ReportedBy.ToLower().Contains(optionalSeachText.ToLower())))).ToList().OrderByDescending(o => o.Updateddate);
              
                   
                }
                else if(orderby == 0)
                {
                    if (string.IsNullOrEmpty(optionalSeachText) && string.IsNullOrEmpty(Type))
                        config.Rows = _entities.SP_GetAllClaimsUpdatedDate(userid, propertyid).ToList().OrderByDescending(o => o.CreatedDate);
                    else if (Type == "All" && string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.SP_GetAllClaimsUpdatedDate(userid, propertyid).Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentLocation.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentDescription.ToLower().Contains(optionalSeachText.ToLower()) || x.ReportedBy.ToLower().Contains(optionalSeachText.ToLower())).ToList().OrderByDescending(o => o.CreatedDate);
                    else if (Type != "All" && string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.SP_GetAllClaimsUpdatedDate(userid, propertyid).Where(x => (x.ClaimType.ToLower().Trim().Contains(Type.ToLower().Trim()))).ToList().OrderByDescending(o => o.CreatedDate);
                    else
                        config.Rows = _entities.SP_GetAllClaimsUpdatedDate(userid, propertyid).Where(x => (x.ClaimType.ToLower() == Type.ToLower() && (x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentLocation.ToLower().Contains(optionalSeachText.ToLower()) || x.IncidentDescription.ToLower().Contains(optionalSeachText.ToLower()) || x.ReportedBy.ToLower().Contains(optionalSeachText.ToLower())))).ToList().OrderByDescending(o => o.CreatedDate);


                }
                config.EtType = EntityType.AllClaims.ToString();
                PropertyInfo[] userprop = typeof(SP_GetAllClaimsUpdatedDate_Result).GetProperties();
                config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                config.Columns = new List<DtableConfigArray>();
                config.Columns.Add(new DtableConfigArray { name = "claimNumber", label = "ID", type = 0, href = "" });
                config.Columns.Add(new DtableConfigArray { name = "propertyName", label = "Property Name", type = 0, href = "" });
                config.Columns.Add(new DtableConfigArray { name = "claimType", label = "Type", type = 0, href = "" });
              
                // config.Columns.Add(new DtableConfigArray { name = "propertyNumber", label = "Property Number", type = 0, href = "" });
                // config.Columns.Add(new DtableConfigArray { name = "propertyManager", label = "Property Manager", type = 0, href = "" });
                //   config.Columns.Add(new DtableConfigArray { name = "propertyAddress", label = "Property Address", type = 0, href = "" });
                //    config.Columns.Add(new DtableConfigArray { name = "incidentLocation", label = "Incident Location", type = 0, href = "" });
                //  config.Columns.Add(new DtableConfigArray { name = "incidentDescription", label = "Incident Description", type = 0, href = "" });
                config.Columns.Add(new DtableConfigArray { name = "incidentDateTime", label = "Incident Date", type = DFieldType.IsDate, href = "" });
                config.Columns.Add(new DtableConfigArray { name = "residentName", label = "Resident Name", type = 0, href = "" });
                config.Columns.Add(new DtableConfigArray { name = "createdDate", label = "Submitted Date", type = DFieldType.IsDate, href = "" });
                config.Columns.Add(new DtableConfigArray { name = "updateddate", label = "Updated Date", type = DFieldType.IsDate, href = "" });
                //  config.Columns.Add(new DtableConfigArray { name = "reportedPhone", label = "Reported Phone", type = DFieldType.IsText, href = "" });
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

                        config.Columns.Add(new DtableConfigArray { name = "userEmail", label = "User Email", type = 0, href = "" });
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

                var update = _entities.updateactivityupdate(_recId);


                if (Type == 'g')
                {
                    //  var _generalclaim = _entities.FormGeneralLiabilityClaims.Where(x => x.GLLId == _recId).FirstOrDefault();

                    var _generalclaim = (from tbl in _entities.FormGeneralLiabilityClaims
                                         join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                         where tbl.GLLId == _recId
                                         select new { tbl, tblprop.PropertyName }).FirstOrDefault();
                    if (_generalclaim != null)
                    {


                        cd.Claim = _generalclaim; }
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
                    if (_damageclaim != null) {

                        

                        cd.Claim = _damageclaim; }
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
                                   select new { tbl.ActivityDescription, ActivityDate = tbl.ActivityDate, tbl.ActivityStatus, tbl.ActivityByName }).ToList();


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
                    { return _formdamageclaim.tbl; }

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

                var AllComments = (from tbl in _entities.FormComments
                                   where tbl.RefFormID == _recId && tbl.RefFormType == formtype
                                   orderby
tbl.CommentDate descending
                                   select tbl).ToList();

                // Get All Attachment for this RowId and Type
                var AllAttachments = (from tbl in _entities.FormAttachments
                                      where tbl.RefFormType == formtype && tbl.RefId == _recId
                                      orderby
tbl.UploadedDate descending
                                      select tbl).ToList();

                var AllActivity = (from tbl in _entities.Activities
                                   where tbl.RecordId == _recId
                                   orderby tbl.ActivityDate descending
                                   select new { tbl.ActivityDescription, ActivityDate = tbl.ActivityDate, tbl.ActivityStatus, tbl.ActivityByName }).ToList();


                cd.Comments = AllComments;
                cd.Attchments = AllAttachments;
                cd.Activity = AllActivity;

                return cd;
            }
        }



        public dynamic GetExportClaim(string Claim, char Type)
        {
            ExportClaim cd = new ExportClaim();

            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                Guid _recId = new Guid(Claim);
                Int16 formtype = 1;
                Guid propid = Guid.NewGuid();

                if (Type == 'g')
                {
                    //  var _generalclaim = _entities.FormGeneralLiabilityClaims.Where(x => x.GLLId == _recId).FirstOrDefault();

                    var _generalclaim = (from tbl in _entities.FormGeneralLiabilityClaims
                                         join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                         where tbl.GLLId == _recId
                                         select new { tbl }).FirstOrDefault();
                    if (_generalclaim != null)
                    {
                        cd.GLC= _generalclaim.tbl;
                        propid = _generalclaim.tbl.PropertyId;
                    }

                    formtype = 2;
                }
                else if (Type == 'm')
                {
                    var _formdamageclaim = (from tbl in _entities.FormMoldDamageClaims
                                            join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                            where tbl.MDLId == _recId
                                            select new { tbl }).FirstOrDefault();

                    //_entities.FormMoldDamageClaims.Where(x => x.MDLId == _recId).FirstOrDefault();
                    if (_formdamageclaim != null)
                    { cd.MDC= _formdamageclaim.tbl;
                        propid = _formdamageclaim.tbl.PropertyId;
                    }

                    formtype = 3;

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
                        cd.PDC= _damageclaim.tbl;
                        propid = _damageclaim.tbl.PropertyId;
                    }

                }

             

                var propertyres = _entities.proc_getpropertydetails(propid).FirstOrDefault();
                cd.Prop = propertyres;

                var AllComments = (from tbl in _entities.FormComments
                                   where tbl.RefFormID == _recId
                                   orderby
tbl.CommentDate descending
                                   select tbl).ToList();

                // Get All Attachment for this RowId and Type
                var AllAttachments = (from tbl in _entities.FormAttachments
                                      where  tbl.RefId == _recId
                                      orderby
tbl.UploadedDate descending
                                      select tbl).ToList();

                var AllActivity = (from tbl in _entities.Activities
                                   where tbl.RecordId == _recId
                                   orderby tbl.ActivityDate descending
                                   select tbl).ToList();


                cd.PrintComments = AllComments;
                cd.PrintAttachments =AllAttachments;
                cd.PrintClaimActivity =AllActivity;

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


        public dynamic GetAllHrFormActivity(Guid _recId,string FormType)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                var AllActivity = (from tbl in _entities.ActivityLogHrForms                                  
                                   where tbl.RefId == _recId && tbl.FormType ==  FormType

                                   orderby tbl.ActivityDate descending
                                   select new { tbl.ActivityDescription, ActivityDate = tbl.ActivityDate, tbl.ActivitySubject, tbl.ActivityId }).ToList();

                return AllActivity;
            }
        }

        public dynamic InsertComment(FormComment _property)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
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
                return new { comments = AllComments, activity = GetAllActivity(_property.RefFormID) };
            }
        }


        public dynamic InsertAttachment(List<FormAttachment> formAttachments)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                foreach (var item in formAttachments)
                {

                FormAttachment _property = item;
                _property.AttachmentId = Guid.NewGuid();
                _property.UploadedDate = DateTime.Now;
                // No record exists create a new property record here
                _entities.FormAttachments.Add(_property);
                // _entities.SaveChanges();
                int i = _entities.SaveChanges();
                }

                var formAttachment = formAttachments[0];
                var AllAttachments = (from tbl in _entities.FormAttachments
                                      where tbl.RefId == formAttachment.RefId && tbl.RefFormType == formAttachment.RefFormType
                                      orderby tbl.UploadedDate descending
                                      select tbl).ToList();
                string Comment = "A new attachment was added by " + formAttachment.UploadedByName;
                LogActivity(Comment, formAttachment.UploadedByName, formAttachment.UploadedBy.ToString(), formAttachment.RefId.ToString(), "New Attachment");
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
                    _activity.IsViewed = false;
                    _entities.Activities.Add(_activity);
                   
                    _entities.SaveChanges();
                }
                catch (Exception ex)
                {

                }


            }

        }

        public void ErrorLog(ErrorLog errorLog)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    _entities.ErrorLogs.Add(errorLog);
                    _entities.SaveChanges();

                }                 
                catch (Exception ex)
                {

                }
        }
        }

        public  void HrLogActivity(string FormType, string RecordId, string ActivitySubject, string ActivityDesc, string UserGuid)
      { 
              using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    ActivityLogHrForm _activity = new ActivityLogHrForm();
    _activity.ActivityId = System.Guid.NewGuid();
                    _activity.ActivityDescription = ActivityDesc;
                    _activity.ActivityDate = DateTime.Now;
                    _activity.ActivityBy = UserGuid;
    _activity.ActivitySubject = ActivitySubject;
                    _activity.RefId = new Guid(RecordId);
                    _activity.FormType = FormType;  
                    _entities.ActivityLogHrForms.Add(_activity);
                    _entities.SaveChanges();
                }
                catch (Exception ex)
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

        public dynamic UpdateWorkflowEmployeeNewHireNotice(string Action, string RefId, string Sign, DateTime? edate, string browser, string ipaddress)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var dlink = new Guid(RefId);
                    var drow = (from tbl in _entities.DynamicLinks
                                where tbl.DynamicLinkId == dlink
                                select tbl).FirstOrDefault();
                    var newhirerow = (from tbl in _entities.EmployeeNewHireNotices
                                      where tbl.EmployeeHireNoticeId == drow.ReferenceId
                                      select tbl).FirstOrDefault();
                    if (Action == "Employee Email")
                    {

                        newhirerow.esignature = Sign;
                        newhirerow.edate = edate;
                        newhirerow.EmployeeSignedDateTime = DateTime.Now;
                    }
                    else
                    {

                        newhirerow.rpmsignature = Sign;
                        newhirerow.rpmdate = edate;
                        newhirerow.RegionalManagerSignedDateTime = DateTime.Now;

                    }
                    drow.OpenStatus = false;
                    drow.BrowserInformation = browser;
                    drow.IpAddress = ipaddress;
                    drow.Clientdatetime = DateTime.Now;

                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();

                    return newhirerow.EmployeeHireNoticeId.ToString();
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }



        public dynamic UpdateWorkflowEmployeeLeaseRider(string Action, string RefId, string Sign, DateTime? edate,string browser,string ipaddress)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var dlink = new Guid(RefId);
                    var drow = (from tbl in _entities.DynamicLinks
                                where tbl.DynamicLinkId == dlink
                                select tbl).FirstOrDefault();
                    var newhirerow = (from tbl in _entities.EmployeeLeaseRaiders
                                      where tbl.EmployeeLeaseRiderId == drow.ReferenceId
                                      select tbl).FirstOrDefault();
                  

                        newhirerow.SignatureOfEmployee = Sign;
                        newhirerow.PositionDate = edate;
                        newhirerow.EmployeeSignedDateTime = DateTime.Now;
                   

                  
                    drow.OpenStatus = false;
                    drow.BrowserInformation = browser;
                    drow.IpAddress = ipaddress;
                    drow.Clientdatetime = DateTime.Now;

                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();

                    return newhirerow.EmployeeLeaseRiderId.ToString();
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }



        public dynamic UpdateWorkflowPayRollStatusChangeNotice(string Action, string RefId, string Sign, DateTime? edate, string browser, string ipaddress)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var dlink = new Guid(RefId);
                    var drow = (from tbl in _entities.DynamicLinks
                                where tbl.DynamicLinkId == dlink
                                select tbl).FirstOrDefault();
                    var newhirerow = (from tbl in _entities.PayrollStatusChangeNotices
                                      where tbl.PayrollStatusChangeNoticeId == drow.ReferenceId
                                      select tbl).FirstOrDefault();


                    newhirerow.ESignature = Sign;
                    newhirerow.EDate = edate;
                    newhirerow.EmployeeSignedDateTime = DateTime.Now;
                                       
                    drow.OpenStatus = false;
                    drow.BrowserInformation = browser;
                    drow.IpAddress = ipaddress;
                    drow.Clientdatetime = DateTime.Now;

                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();

                    return newhirerow.PayrollStatusChangeNoticeId.ToString();
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

        public dynamic InsertEmployeeNewHireNotice(EmployeeNewHireNotice formAttachment, string Type)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {
                    EmployeeNewHireNotice _property = formAttachment;
                    //var propid = _property.ModifiedUser;

                    // get Regional Manager Id for the Property

                    if(Type=="Insert")
                    {
                        //_property.EmployeeHireNoticeId = new Guid(_property.ModifiedUser.ToString());
                        //_property.ModifiedUser = null;
                   
                    _property.CreatedDateTime = DateTime.Now;
                    // No record exists create a new property record here


                    var regionmrg = (from tbl in _entities.Properties
                                     where tbl.PropertyId == _property.ModifiedUser
                                     select tbl.RegionalManager).FirstOrDefault();

                    _property.ModifiedUser = null;
                    _property.RegionaManager = regionmrg;
                    _property.PmSignedDateTime = DateTime.Now;

                    _entities.EmployeeNewHireNotices.Add(_property);
                    _entities.SaveChanges();

                    }
                    else
                    {
                        var res = (from tbl in _entities.EmployeeNewHireNotices
                                   where tbl.EmployeeHireNoticeId == _property.EmployeeHireNoticeId
                                   select tbl).FirstOrDefault();
                        res.ModifiedDateTime = DateTime.Now;
                        res.StartDate = _property.StartDate;
                        res.EmployeeName = _property.EmployeeName;
                        //   res.EmployeeSocialSecuirtyNumber = HttpContext.Current.Request.Params["securitynumber"].ToString();
                        res.EmailAddress = _property.EmailAddress;
                        res.Manager = _property.Manager;
                        res.Location = _property.Location;
                        res.iscorporate = _property.iscorporate;
                        res.IsRejected = _property.IsRejected ;
                      
                            res.IsResumitted = true;
                        res.ResubmittedBy = _property.ResubmittedBy;
                            res.ResubmittedDateTime = DateTime.Now;
                        

                        // res.EmployeeHireNoticeId = System.Guid.NewGuid();
                        res.Position = _property.Position ;
                        res.Position_Exempt = _property.Position_Exempt;
                        res.Position_NonExempt = _property.Position_NonExempt ;
                        res.Status = _property.Status; ;
                        res.Sal_Time = _property.Sal_Time; ;
                        res.Wage_Salary = _property.Wage_Salary; ;

                        if (res.iscorporate == false)
                        {
                            res.La_Property1 = _property.La_Property1;
                            res.La_Property1_Per = _property.La_Property1_Per;
                            if (res.La_Property1_Per != 100)
                            {
                                if (!String.IsNullOrEmpty(_property.La_Property2_Per.ToString()))
                                {
                                    res.La_Property2 = _property.La_Property2;
                                    res.La_Property2_Per = _property.La_Property2_Per;

                                }
                                else
                                {
                                    res.La_Property2 = "";
                                    res.La_Property2_Per = null;
                                }

                                if (!String.IsNullOrEmpty(_property.La_Property3))
                                {
                                    res.La_Property3 = _property.La_Property3;
                                    res.La_Property3_Per = _property.La_Property3_Per;
                                }
                                else
                                {
                                    res.La_Property3 = "";
                                    res.La_Property3_Per = null;
                                }
                            }
                            else
                            {
                                res.La_Property2 = "";
                                res.La_Property2_Per = null;
                                res.La_Property3 = "";
                                res.La_Property3_Per = null;
                            }

                        }
                        else
                        {
                            res.La_Property1 = "";
                            res.La_Property1_Per = null;
                            res.La_Property2 = "";
                            res.La_Property2_Per = null;
                            res.La_Property3 = "";
                            res.La_Property3_Per = null;
                        }

                        res.Status = _property.Status;
                        res.AdditionalText = _property.AdditionalText;
                        res.EmployeeSignedDateTime = null;
                        res.RegionalManagerSignedDateTime = null;
                            res.kitordered = _property.kitordered;
                        //   res.boardingcallscheduled = Convert.ToDateTime(HttpContext.Current.Request.Params["callscheduled"]);
                        res.Allocation = _property.Allocation;
                        res.esignature = null;
                        res.edate = null;

                        res.msignature =_property.msignature;
                            res.mdate = _property.mdate;

                        res.rpmsignature = null;
                            res.rpmdate = null;
                        res.PmSignedDateTime = _property.PmSignedDateTime;
                        int i1 = _entities.SaveChanges();
                    }
                       
                                   
                    return new { Error = false, ErrorMsg = "", InsertedId = _property.EmployeeHireNoticeId };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }


        public string GetPropertyManager(Guid PropertyId)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {

                    var regionmrg = (from tbl in _entities.Properties
                                     join tblcontact in _entities.Contacts on tbl.PropertyManager equals tblcontact.ContactId
                                     where tbl.PropertyId == PropertyId
                                     select new { tblcontact.FirstName, tblcontact.LastName }).FirstOrDefault();
                    if (regionmrg != null)
                    {
                        return regionmrg.FirstName + " " + regionmrg.LastName;
                    }
                    else
                    {
                        return "No Manager";

                    }
                }
                catch (Exception ex)
                {
                    return "Error :" + ex.Message;
                }
            }




        }


      public  dynamic GetNewHireRejectionDetails(string Refid)
      {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var riderid = new Guid(Refid);

                    var res = (from tbl in _entities.EmployeeNewHireNotices
                               join tbluser in _entities.SiteUsers on tbl.RejectedBy equals tbluser.UserId
                               where tbl.EmployeeHireNoticeId == riderid
                               select new { tbl.RejectedReason,tbluser.FirstName,tbluser.LastName, tbl.RejectedDateTime }).FirstOrDefault();

                    // get meta data of pm

                    var dl1 = (from tbl in _entities.DynamicLinks
                               where tbl.FormType == "NewHire" && tbl.Action == "PM Email" && tbl.ReferenceId == riderid
                               select tbl).FirstOrDefault();

                    // get meta data of emp

                    var dl2 = (from tbl in _entities.DynamicLinks
                               where tbl.FormType == "NewHire" && tbl.Action == "Employee Email" && tbl.ReferenceId == riderid
                               select tbl).FirstOrDefault();
                    // get meta data of regional email

                    var dl3 = (from tbl in _entities.DynamicLinks
                               where tbl.FormType == "NewHire" && tbl.Action == "Regional Email" && tbl.ReferenceId == riderid
                               select tbl).FirstOrDefault();

                    return new { newhiredetails=res,pm=dl1,emp=dl2,regional=dl3 };
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

                    if (res.iscorporate == false)
                    {
                        var propid = new Guid(res.Location);
                        var propname = (from tbl in _entities.Properties
                                        where tbl.PropertyId == propid
                                        select tbl.PropertyName).FirstOrDefault();
                        res.Location = propname;
                    }

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
                               select new { tbl} ).FirstOrDefault();
                    if(res!= null)
                    {
                        if(res.tbl.IsCorporate == false)
                        {
                            if(res.tbl.Property.ToString() != "")
                            {
                                var propname = (from tbl in _entities.Properties
                                                where tbl.PropertyId == res.tbl.Property
                                                select tbl.PropertyName).FirstOrDefault();
                                res.tbl.Notes2 = propname;
                            }
                           
                        }
                    }
                    // res.tbl.Notes2 = res.PropertyName;
                    if (res.tbl.Notes1 == null)
                        res.tbl.Notes1 = "";
                    return res.tbl;
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





        public dynamic InsertMileageLog(MileageLogHeader mlh, List<MileageLogDetail> mld)
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


                    return new { header = res, details = res2 };
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


        public dynamic InsertResidentReferralRequest(ResidentReferalSheet mlh)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {
                    ResidentReferalSheet _property = mlh;

                    // No record exists create a new property record here
                    _entities.ResidentReferalSheets.Add(_property);
                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();


                    return new { Error = false, ErrorMsg = "", InsertedId = _property.ResidentReferalId };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }


        public dynamic GetResidentReferralRequest(Guid riderid)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {
                    var res = (from tbl in _entities.ResidentReferalSheets
                               where tbl.ResidentReferalId == riderid
                               select tbl).FirstOrDefault();


                    return res;
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }


        public dynamic InsertResidentReferralContact(ResidentContactInformation mlh, List<ResidentContactInformation_Residents> rrs, List<ResidentContactInformation_OtherOccupants> ors, List<ResidentContactInformation_Vehicles> vhs)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {

                try
                {
                    ResidentContactInformation _property = mlh;

                    // No record exists create a new property record here
                    _entities.ResidentContactInformations.Add(_property);

                    foreach (var item in rrs)
                    {
                        _entities.ResidentContactInformation_Residents.Add(item);
                    }

                    foreach (var item in ors)
                    {
                        _entities.ResidentContactInformation_OtherOccupants.Add(item);
                    }

                    foreach (var item in vhs)
                    {
                        _entities.ResidentContactInformation_Vehicles.Add(item);
                    }

                    // _entities.SaveChanges();
                    int i = _entities.SaveChanges();


                    return new { Error = false, ErrorMsg = "", InsertedId = _property.Contactid };
                }
                catch (Exception ex)
                {
                    return new { Error = true, ErrorMsg = ex.Message, InsertedId = "" };
                }
            }
        }


        public PrintResidentContact GetResidentReferralContact(Guid riderid)
        {
            using (CarrollFormsEntities _entities = new CarrollFormsEntities())
            {
                try
                {

                    PrintResidentContact prc = new PrintResidentContact();

                    var resident = (from tbl in _entities.ResidentContactInformations
                                    where tbl.Contactid == riderid
                                    select tbl).FirstOrDefault();

                    var res1 = (from tbl in _entities.ResidentContactInformation_Residents
                                where tbl.ResidentContactInformationId == riderid
                                select tbl).ToList();

                    var res2 = (from tbl in _entities.ResidentContactInformation_OtherOccupants
                                where tbl.ResidentContactInformationId == riderid
                                select tbl).ToList();

                    var res3 = (from tbl in _entities.ResidentContactInformation_Vehicles
                                where tbl.ResidentContactInformationId == riderid
                                select tbl).ToList();

                    prc.Contactid = resident.Contactid;
                    prc.Apartment = resident.Apartment;
                    prc.Building = resident.Building;
                    prc.PropertyName = resident.PropertyName;
                    prc.ReturnEmail = resident.ReturnEmail;
                    prc.Fax1 = resident.Fax1;
                    prc.Fax11 = resident.Fax11;
                    prc.Fax2 = resident.Fax2;
                    prc.Fax22 = resident.Fax22;
                    prc.InsuranceDeclaration = resident.InsuranceDeclaration;
                    prc.Em_name = resident.Em_name;
                    prc.Em_Address = resident.Em_Address;
                    prc.Em_Phone = resident.Em_Phone;
                    prc.Em_Relation = resident.Em_Relation;
                    prc.ResidentSignDate1 = resident.ResidentSignDate1;
                    prc.ResidentSignDate2 = resident.ResidentSignDate2;
                    prc.ResidentSingature1 = resident.ResidentSingature1;
                    prc.ResidentSingature2 = resident.ResidentSingature2;

                    List<ResidentReferralResidents> rrs = new List<ResidentReferralResidents>();

                    foreach (var item in res1)
                    {
                        rrs.Add(new ResidentReferralResidents { Name = item.Name, MobilePhone = item.MobilePhone, Email = item.Email, Home_Work = item.Home_Work, Home_Work_Phone = item.Home_Work_Phone, CurrentEmployer = item.CurrentEmployer, Position = item.Position });
                    }
                    prc.rrs = rrs;

                    List<ResidentReferralOthers> ros = new List<ResidentReferralOthers>();

                    foreach (var item in res2)
                    {
                        ros.Add(new ResidentReferralOthers { Name = item.Name, DOB = item.DOB });

                    }

                    prc.ros = ros;


                    List<ResidentReferralVehicles> rvs = new List<ResidentReferralVehicles>();

                    foreach (var item in res3)
                    {
                        rvs.Add(new ResidentReferralVehicles { Make = item.Make, Model = item.Model, Type = item.Type, Year = item.Year, Color = item.Color, LicensePlate = item.LicensePlate, LicensePlatState = item.LicensePlatState });
                    }

                    prc.rvs = rvs;



                    return prc;
                }
                catch (Exception ex)
                {
                    return new PrintResidentContact { };
                }
            }
        }
        public dynamic GetHrFormCount()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                var ClaimCounts = new { LeaseCount = 0, PayrollCount = 0, EmployeeSeparationCount = 0, NewHireCount = 0 };

                var leasecount = (from tbl in _entities.EmployeeLeaseRaiders
                                  select tbl).Count();
                var payrollcount = (from tbl in _entities.PayrollStatusChangeNotices
                                    select tbl).Count();
                var seperationcount = (from tbl in _entities.NoticeOfEmployeeSeperations
                                       select tbl).Count();
                var newhirecount = (from tbl in _entities.EmployeeNewHireNotices
                                    select tbl).Count();
                return new { LeaseCount = leasecount, PayRollCount = payrollcount, SeparationCount = seperationcount, HireCount = newhirecount };
            }
        }

        public dynamic GetAllMileageForms(string FormType, Guid userid, string optionalSeachText)
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


                else if (FormType == "ResidentReferal")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallresidentreferals(userid).ToList();
                    else
                        config.Rows = _entities.proc_getallresidentreferals(userid).Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.ResidentName.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(proc_getallresidentreferals_Result).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "propertyName", label = "Property Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "residentName", label = "Resident Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "referredResident", label = "Resident Referral", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "unitNumber", label = "UnitNumber", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "referalBonus", label = "ReferralBonus", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }

                else if (FormType == "ResidentContact")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.Proc_getallresidentcontacts().ToList();
                    else
                        config.Rows = _entities.Proc_getallresidentcontacts().Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.ResidentName.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(Proc_getallresidentcontacts_Result).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "propertyName", label = "Property Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "building", label = "Building Number", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "apartment", label = "Apartment Number", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "residentName", label = "Resident #1 Name", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "insuranceDeclaration", label = "Renter's Insurance on File", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }
                return config;

            }

        }


        public dynamic GetAllHrForms(Guid? userid, string FormType, string optionalSeachText)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;

                var config = new Config { };

                if (FormType == "LeaseRider")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallemployeeleaseriders(userid).ToList();
                    else
                        config.Rows = _entities.proc_getallemployeeleaseriders(userid).Where(x => x.Community.ToLower().Contains(optionalSeachText.ToLower()) || x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(proc_getallemployeeleaseriders_Result).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "sequenceNumber", label = "ID", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "community", label = "Property", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Employee", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "date", label = "Date", type = DFieldType.IsDate, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "apartmentMarketRentalValue", label = "Rental Value", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "employeeMonthlyRent", label = "Monthly Rent", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "rentalPaymentResidencyAt", label = "Rental Payment At", type = 0, href = "" });                   
                    //config.Columns.Add(new DtableConfigArray { name = "position", label = "Position", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                 
                    config.Columns.Add(new DtableConfigArray { name = "managerSigned", label = "Manager Signed", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "empSigned", label = "Employee Signed", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "notes", label = "Activity", type = DFieldType.IsText, href = "" });

                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }

                else if (FormType == "Requisition Request")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallrequisitionrequests(userid).ToList();
                    else
                        config.Rows = _entities.proc_getallrequisitionrequests(userid).Where(x => x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.RequestorName.ToLower().Contains(optionalSeachText.ToLower()) || x.RequestorPosition.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(RequisitionRequest).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();
                    config.Columns.Add(new DtableConfigArray { name = "sequenceNumber", label = "ID", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "propertyName", label = "Property Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "requestorName", label = "Requestor Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "requestorPosition", label = "Position", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "post", label = "How Many", type = DFieldType.IsText, href = "" });
                   // config.Columns.Add(new DtableConfigArray { name = "type", label = "Type", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "userName", label = "Created By", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "requisitionRequestId", label = "Id", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "createdDateTime", label = "Created Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "notes", label = "Activity", type = DFieldType.IsText, href = "" });

                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }
                else if (FormType == "PayRollChange")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallpayrollstatuschange(userid).ToList();
                    else
                        config.Rows = _entities.proc_getallpayrollstatuschange(userid).Where(x => x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower()) || x.ChangeEffectiveDate.Value.ToShortDateString().ToLower().Contains(optionalSeachText.ToLower()) || x.TypeOfChange.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(PayrollStatusChangeNotice).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();
                    config.Columns.Add(new DtableConfigArray { name = "sequenceNumber", label = "ID", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Employee Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "changeEffectiveDate", label = "Effective Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "typeOfChange", label = "Type Of Change", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "createdDateTime", label = "Manager Signed", type = DFieldType.IsText, href = "" });

                    config.Columns.Add(new DtableConfigArray { name = "empSigned", label = "Employee Signed", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "notes", label = "Activity", type = DFieldType.IsText, href = "" });

                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }
                else if (FormType == "EmployeeSeparation")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallnoticeofemployeeseparation(userid).ToList();
                    else
                        config.Rows = _entities.proc_getallnoticeofemployeeseparation(userid).Where(x => x.EffectiveDateOfChange.Value.ToString().ToLower().Contains(optionalSeachText.ToLower()) || x.EligibleForReHire.ToString().ToLower().Contains(optionalSeachText.ToLower()) || x.PropertyName.ToLower().Contains(optionalSeachText.ToLower()) || x.PropertyNumber.ToLower().Contains(optionalSeachText.ToLower()) || x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower()) || x.JobTitle.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(proc_getallnoticeofemployeeseparation_Result).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();
                    config.Columns.Add(new DtableConfigArray { name = "sequenceNumber", label = "ID", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "propertyName", label = "Property Name", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Employee Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "jobTitle", label = "Employee Position", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "reason", label = "Reason", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "effectiveDateOfChange", label = "Effective Date", type = DFieldType.IsDate, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "timeStamp", label = "Date Signed", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "notes", label = "Activity", type = DFieldType.IsText, href = "" });

                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }
                else if (FormType == "New Hire Notice")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.proc_getallemployeenewhirenoticenew(userid).ToList();
                    else
                        config.Rows = _entities.proc_getallemployeenewhirenoticenew(userid).Where(x => x.EmployeeName.ToLower().Contains(optionalSeachText.ToLower()) || x.StartDate.Value.ToShortDateString().ToString().ToLower().Contains(optionalSeachText.ToLower()) || x.Location.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(proc_getallemployeenewhirenoticenew_Result).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();
                    config.Columns.Add(new DtableConfigArray { name = "sequenceNumber", label = "ID", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "employeeName", label = "Employee Name", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "startDate", label = "Start Date", type = DFieldType.IsDate, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "employeeSocialSecuirtyNumber", label = "Social SecuirtyNumber", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "emailAddress", label = "EmailAddress", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "manager", label = "Manager", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "location", label = "Location", type = 0, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "position_Exempt", label = "Position_Exempt", type = DFieldType.IsText, href = "" });
                    //config.Columns.Add(new DtableConfigArray { name = "position_NonExempt", label = "Position_NonExempt", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "position", label = "Position", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pmSigned", label = "Manager Signed", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "empSigned", label = "Employee Signed", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "rpmSigned", label = "Regional Signed", type = DFieldType.IsText, href = "" });
                    
                    config.Columns.Add(new DtableConfigArray { name = "rejectionStatus", label = "RejectionStatus", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "notes", label = "Activity", type = DFieldType.IsText, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "printOption", label = "Print", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "pdfOption", label = "Save", type = DFieldType.IsText, href = "" });

                }
                if (FormType == "ErrorLog")
                {
                    if (string.IsNullOrEmpty(optionalSeachText))
                        config.Rows = _entities.ErrorLogs.ToList();
                    else
                        config.Rows = _entities.ErrorLogs.Where(x => x.Page.ToLower().Contains(optionalSeachText.ToLower()) || x.UserName.ToLower().Contains(optionalSeachText.ToLower()) || x.Error.ToLower().Contains(optionalSeachText.ToLower())).ToList();

                    config.EtType = EntityType.AllClaims.ToString();
                    PropertyInfo[] userprop = typeof(ErrorLog).GetProperties();
                    config.PkName = FirstChartoLower(userprop.ToList().FirstOrDefault().Name);
                    config.Columns = new List<DtableConfigArray>();

                    config.Columns.Add(new DtableConfigArray { name = "userName", label = "User", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "page", label = "Page", type = 0, href = "" });
                    config.Columns.Add(new DtableConfigArray { name = "error", label = "Error", type = 0, href = "" });
                   config.Columns.Add(new DtableConfigArray { name = "datetime", label = "Date", type = DFieldType.IsDate, href = "" });                   

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


        public List<proc_getcontactsforexcel_Result1> GetAllContactsForExcel()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                return _entities.proc_getcontactsforexcel().ToList();
            }
        }

        public List<proc_getequitypartnersforexcel_Result1> GetAllEquityPartnersForExcel()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                return _entities.proc_getequitypartnersforexcel().ToList();
            }

        }


        public List<proc_getpropertiesforexcelupdate_Result> GetAllPropertiesForExcel()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                return _entities.proc_getpropertiesforexcelupdate().ToList();
            }
        }


        public List<CarrollPayPeriod> GetAllCarrollPayPerilds()
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {

                //us
                var propertyres = _entities.CarrollPayPeriods.ToList();



                if (propertyres != null)
                    return propertyres;
                else
                    return null;
            }
        }

        public dynamic ImportContactTableFromExcel(DataTable dt)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var isnewrow = false;

                    if (row[0].ToString() != "")
                    {
                        var uid = new Guid(row[0].ToString());
                        var change = false;

                        var contact = (from tbl in _entities.Contacts
                                       where tbl.ContactId == uid
                                       select tbl).FirstOrDefault();
                        if(contact == null)
                        {
                            isnewrow = true;


                        }
                        else
                        {

                            if (contact.FirstName != row[1].ToString().Trim())
                            {
                                contact.FirstName = row[1].ToString().Trim();
                                change = true;
                            }
                            if (contact.LastName != row[2].ToString().Trim())
                            {
                                contact.LastName = row[2].ToString().Trim();
                                change = true;
                            }
                            if (contact.Title != row[3].ToString().Trim())
                            {
                                contact.Title = row[3].ToString().Trim();
                                change = true;
                            }
                            if (contact.Email != row[4].ToString().Trim())
                            {
                                contact.Email = row[4].ToString().Trim();
                                change = true;
                            }
                            if (contact.Phone != row[5].ToString().Trim())
                            {
                                contact.Phone = row[5].ToString().Trim();
                                change = true;
                            }

                            if (change == true)
                            {
                                int i = _entities.SaveChanges();
                                }


                        
                        }

                    }

                    if(row[0].ToString() == "" || isnewrow== true )
                    {
                        Contact cc = new Contact();
                        cc.ContactId = Guid.NewGuid();
                        cc.FirstName = row[1].ToString().Trim();
                        cc.LastName = row[2].ToString().Trim();
                        cc.Title = row[3].ToString().Trim();
                        cc.Email = row[4].ToString().Trim();
                        cc.Phone = row[5].ToString().Trim();
                        cc.CreatedDate = DateTime.Now;
                        cc.CreatedByName = "Excel Upload";
                        _entities.Contacts.Add(cc);
                        _entities.SaveChanges();
                    }

                }


            }

            return true;

        }
        public dynamic ImportEquityPartnerTableFromExcel(DataTable dt)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var isnewrow = false;
                    var change = false;


                    if (row[0].ToString() != "")
                    {
                        var uid = new Guid(row[0].ToString());
                       
                       

                        var contact = (from tbl in _entities.EquityPartners
                                       join c in _entities.Contacts on tbl.ContactId equals c.ContactId into bg
                                       from x in bg.DefaultIfEmpty()
                                       where tbl.EquityPartnerId == uid
                                       select new { tbl, ContactName =( x == null? string.Empty : x.FirstName+" "+x.LastName)  }).FirstOrDefault();
                        if (contact == null)
                            isnewrow = true;
                        else
                        {

                                            if (contact.tbl.PartnerName != row[1].ToString().Trim())
                                            {
                                                contact.tbl.PartnerName = row[1].ToString().Trim();
                                                change = true;
                                            }

                                            if (contact.tbl.AddressLine1 != row[2].ToString().Trim())
                                            {
                                                contact.tbl.AddressLine1 = row[2].ToString().Trim();
                                                change = true;
                                            }

                                            if (contact.tbl.AddressLine2 != row[3].ToString().Trim())
                                            {
                                                contact.tbl.AddressLine2 = row[3].ToString().Trim();
                                                change = true;
                                            }

                                            if (contact.tbl.City != row[4].ToString().Trim())
                                            {
                                                contact.tbl.City = row[4].ToString().Trim();
                                                change = true;
                                            }

                                            if (contact.tbl.State != row[5].ToString().Trim())
                                            {
                                                contact.tbl.State = row[5].ToString().Trim();
                                                change = true;
                                            }

                                            if (contact.tbl.ZipCode != row[6].ToString().Trim())
                                            {
                                                contact.tbl.ZipCode = row[6].ToString().Trim();
                                                change = true;
                                            }

                            if (!string.IsNullOrEmpty(row[7].ToString()))
                            {

                                if (contact.ContactName != row[7].ToString().Trim() && !contact.ContactName.Contains(row[7].ToString()))
                                {
                                    Contact c = new Contact();
                                    c.ContactId = Guid.NewGuid(); ;
                                    var sp = row[7].ToString().Split(new char[0]);

                                    c.FirstName = row[7].ToString().Split(new char[0])[0].ToString();
                                    if (sp.Length > 1)
                                    {
                                        c.LastName = row[7].ToString().Replace(c.FirstName, "");

                                        //if (row[7].ToString().Split(new char[0])[1] != null)
                                        //{
                                        //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                        //}

                                    }
                                    c.Title = "Equity Partner Contact";
                                    c.CreatedBy = contact.tbl.CreatedBy;
                                    c.CreatedDate = DateTime.Now;
                                    c.CreatedByName = contact.tbl.CreatedByName;
                                    _entities.Contacts.Add(c);
                                    _entities.SaveChanges();
                                    contact.tbl.ContactId = c.ContactId;
                                    change = true;
                                }

                                if (change == true)
                                {
                                    int i = _entities.SaveChanges();
                                }
                            }
                            else
                                contact.tbl.ContactId = null;
                        }


                      


                    }

                    if (row[0].ToString() == "" || isnewrow == true)
                    {
                        // check contact exist of not 
                        Guid? contactid;

                        var val = row[7].ToString().Trim();

                        if (!string.IsNullOrEmpty(val))
                        {

                            var cct = (from tbl in _entities.Contacts

                                       where tbl.FirstName.Contains(val) || tbl.LastName.Contains(val)
                                       select tbl.ContactId).FirstOrDefault();
                            if (cct == null)
                            {
                                contactid = cct;
                            }
                            else
                            {
                                Contact c = new Contact();
                                c.ContactId = Guid.NewGuid(); ;
                                var sp = row[7].ToString().Split(new char[0]);

                                c.FirstName = row[7].ToString().Split(new char[0])[0].ToString();
                                c.Email = "";
                                c.IsActive = true;
                                if (sp.Length > 1)
                                {
                                    c.LastName = row[7].ToString().Replace(c.FirstName, "");

                                    //if (row[7].ToString().Split(new char[0])[1] != null)
                                    //{
                                    //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                    //}

                                }
                                c.Title = "Equity Partner Contact";
                                //   c.CreatedBy = contact.tbl.CreatedBy;
                                c.CreatedDate = DateTime.Now;
                                //   c.CreatedByName = contact.tbl.CreatedByName;
                                _entities.Contacts.Add(c);
                                _entities.SaveChanges();
                                contactid = c.ContactId;

                            }
                        }
                        else
                            contactid = null;

                        EquityPartner cc = new EquityPartner();
                        cc.EquityPartnerId = Guid.NewGuid();
                        cc.PartnerName = row[1].ToString().Trim();
                        cc.AddressLine1 = row[2].ToString().Trim();
                        cc.AddressLine2 = row[3].ToString().Trim();
                        cc.City = row[4].ToString().Trim();
                        cc.State = row[5].ToString().Trim();
                        cc.ZipCode = row[6].ToString().Trim();
                        cc.ContactId = contactid;
                        cc.CreatedDate = DateTime.Now;
                        cc.CreatedByName = "Excel Upload";
                        _entities.EquityPartners.Add(cc);
                        _entities.SaveChanges();
                    }


                }


            }
            return true;

        }
            

     public  dynamic ImportPropertiesTableFromExcel(DataTable dt)
    {

            using (CarrollFormsEntities _entities = DBEntity)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var change = false;
                    var isnewrow = false;

                    if (row[0].ToString() != "")
                    {
                        var uid = new Guid(row[0].ToString());

                        var propnumber = Convert.ToInt32(row[7].ToString());

                        var contact = (from tbl in _entities.Properties                                     
                                       where tbl.PropertyNumber == propnumber
                                     select tbl).FirstOrDefault();

                        if (contact == null)
                            isnewrow = true;
                        else
                        {
                            var othercontact = _entities.proc_getpropertydetailsforupdate(uid).FirstOrDefault();

                            // check if name not changed, if changed check already exists, if exists, get contactid,
                            // if not exists create new contact id, update id init, 

                            // if name not changed, check contact number changed, if changed update in corresponding contact from property contact id

                            // check if changed

                            // For VP

                            if (!string.IsNullOrEmpty(row[1].ToString()))
                            {
                                if (othercontact.VP.ToLower().Trim() != row[1].ToString().ToLower().Trim())
                                {
                                    // if changed, check contact exist with given name

                                    var searchstr = row[1].ToString().Trim().ToLower();
                                    var checkexists55 = _entities.proc_checkcontactexists(searchstr).FirstOrDefault();
                                    if (checkexists55 != null)
                                    {
                                        var chkcontact = (from tbl in _entities.Contacts
                                                          where tbl.ContactId == checkexists55.ContactId
                                                          select tbl).FirstOrDefault();

                                        if (chkcontact != null)
                                        {
                                            contact.VicePresident = chkcontact.ContactId;
                                            chkcontact.Phone = row[2].ToString();
                                            change = true;
                                        }
                                    }
                                    else
                                    {
                                        // create new contact with this Name and phone number and update contact id as vp for property table

                                        Contact c = new Contact();
                                        c.ContactId = Guid.NewGuid(); ;
                                        var sp = row[1].ToString().Split(new char[0]);

                                        c.FirstName = row[1].ToString().Split(new char[0])[0].ToString();
                                        if (sp.Length > 1)
                                        {
                                            c.LastName = row[1].ToString().Replace(c.FirstName, "");

                                            //if (row[7].ToString().Split(new char[0])[1] != null)
                                            //{
                                            //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                            //}

                                        }
                                        c.Title = "Vice President";
                                        c.CreatedBy = contact.CreatedBy;
                                        c.CreatedDate = DateTime.Now;
                                        c.CreatedByName = contact.CreatedByName;
                                        _entities.Contacts.Add(c);
                                        _entities.SaveChanges();
                                        contact.VicePresident = c.ContactId;
                                        change = true;

                                    }
                                }
                                else if (othercontact.VP_.ToLower().Trim() != row[2].ToString().ToLower().Trim())
                                {
                                    // get contact and update mobile number

                                    var cn = (from tbl in _entities.Contacts
                                              where tbl.ContactId == contact.VicePresident
                                              select tbl).FirstOrDefault();

                                    if (cn != null)
                                    {
                                        cn.Phone = row[2].ToString().Trim();
                                    }
                                    _entities.SaveChanges();
                                }
                            }
                            else
                            {
                                contact.VicePresident = null;
                            }



                            // For RVP

                            if (!string.IsNullOrEmpty(row[3].ToString()))
                            {


                                if (othercontact.RVP.ToLower().Trim() != row[3].ToString().ToLower().Trim())
                                {
                                    // if changed, check contact exist with given name
                                    var searchstr = row[3].ToString().Trim().ToLower();
                                    var checkexists55 = _entities.proc_checkcontactexists(searchstr).FirstOrDefault();
                                    if (checkexists55 != null)
                                    {

                                        var chkcontact = (from tbl in _entities.Contacts
                                                          where tbl.ContactId == checkexists55.ContactId
                                                          select tbl).FirstOrDefault();

                                        if (chkcontact != null)
                                        {
                                            contact.RegionalVicePresident = chkcontact.ContactId;
                                            chkcontact.Phone = row[4].ToString();
                                            change = true;
                                        }
                                    }
                                    else
                                    {
                                        // create new contact with this Name and phone number and update contact id as vp for property table

                                        Contact c = new Contact();
                                        c.ContactId = Guid.NewGuid(); ;
                                        var sp = row[3].ToString().Split(new char[0]);

                                        c.FirstName = row[3].ToString().Split(new char[0])[0].ToString();
                                        if (sp.Length > 1)
                                        {
                                            c.LastName = row[3].ToString().Replace(c.FirstName, "");

                                            //if (row[7].ToString().Split(new char[0])[1] != null)
                                            //{
                                            //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                            //}

                                        }
                                        c.Title = "Regional Vice President";
                                        c.CreatedBy = contact.CreatedBy;
                                        c.CreatedDate = DateTime.Now;
                                        c.CreatedByName = contact.CreatedByName;
                                        _entities.Contacts.Add(c);
                                        _entities.SaveChanges();
                                        contact.RegionalVicePresident = c.ContactId;
                                        change = true;

                                    }
                                }
                            }
                            else
                            {
                                contact.RegionalVicePresident = null;
                            }

                            if (othercontact.RVP_.ToLower().Trim() != row[4].ToString().ToLower().Trim())
                            {
                                // get contact and update mobile number

                                var cn = (from tbl in _entities.Contacts
                                          where tbl.ContactId == contact.RegionalVicePresident
                                          select tbl).FirstOrDefault();

                                if (cn != null)
                                {
                                    cn.Phone = row[4].ToString().Trim();
                                }
                                _entities.SaveChanges();
                            }



                            // For Regional Manager

                            if (!string.IsNullOrEmpty(row[5].ToString()))
                            {

                                if (othercontact.RM.ToLower().Trim() != row[5].ToString().ToLower().Trim())
                                {
                                    // if changed, check contact exist with given name
                                 

                                        var searchstr = row[5].ToString().Trim().ToLower();
                                    var checkexists55 = _entities.proc_checkcontactexists(searchstr).FirstOrDefault();
                                    if (checkexists55 != null)
                                    {
                                        var chkcontact = (from tbl in _entities.Contacts
                                                          where tbl.ContactId == checkexists55.ContactId
                                                          select tbl).FirstOrDefault();

                                        if (chkcontact != null)
                                        {
                                            contact.RegionalManager = chkcontact.ContactId;
                                            chkcontact.Phone = row[6].ToString();
                                            change = true;
                                        }
                                    }
                                    else
                                    {
                                        // create new contact with this Name and phone number and update contact id as vp for property table

                                        Contact c = new Contact();
                                        c.ContactId = Guid.NewGuid(); ;
                                        var sp = row[5].ToString().Split(new char[0]);

                                        c.FirstName = row[5].ToString().Split(new char[0])[0].ToString();
                                        if (sp.Length > 1)
                                        {
                                            c.LastName = row[5].ToString().Replace(c.FirstName, "");

                                            //if (row[7].ToString().Split(new char[0])[1] != null)
                                            //{
                                            //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                            //}

                                        }
                                        c.Title = "Regional Manager ";
                                        c.CreatedBy = contact.CreatedBy;
                                        c.CreatedDate = DateTime.Now;
                                        c.CreatedByName = contact.CreatedByName;
                                        _entities.Contacts.Add(c);
                                        _entities.SaveChanges();
                                        contact.RegionalManager = c.ContactId;
                                        change = true;

                                    }
                                }

                            }
                            else
                            {
                                contact.RegionalManager = null;
                            }

                            if (othercontact.RM_.ToLower().Trim() != row[6].ToString().ToLower().Trim())
                            {
                                // get contact and update mobile number

                                var cn = (from tbl in _entities.Contacts
                                          where tbl.ContactId == contact.RegionalManager
                                          select tbl).FirstOrDefault();

                                if (cn != null)
                                {
                                    cn.Phone = row[6].ToString().Trim();
                                }
                                _entities.SaveChanges();
                            }



                            if (contact.PropertyNumber.ToString() != row[7].ToString().Trim())
                            {
                                if(!string.IsNullOrEmpty(row[7].ToString()))
                                contact.PropertyNumber = Convert.ToInt16(row[7].ToString().Trim());
                                change = true;
                            }

                            if (contact.Units.ToString() != row[8].ToString().Trim())
                            {
                                if (!string.IsNullOrEmpty(row[8].ToString()))
                                    contact.Units = Convert.ToInt16(row[8].ToString().Trim());
                                change = true;
                            }

                            if (contact.PropertyName.ToLower().Trim() != row[9].ToString().ToLower().Trim())
                            {
                                contact.PropertyName = row[9].ToString().Trim();
                                change = true;
                            }

                            if (!string.IsNullOrEmpty(row[11].ToString()))
                            {

                                if (othercontact.EquityPartner.ToLower().Trim() != row[10].ToString().ToLower().Trim())
                                {
                                    // check if exist, exist udpate partnerid, if not then create new equitypartner, update id
                                    var searchstr = row[10].ToString().Trim().ToLower();

                                    var otherpartner = (from tbl in _entities.EquityPartners
                                                        where tbl.PartnerName.ToLower().Contains(searchstr)
                                                        select tbl).FirstOrDefault();

                                    if (otherpartner != null)
                                    {
                                        contact.EquityPartner = otherpartner.EquityPartnerId;
                                    }
                                    else
                                    {
                                        // create new partner and update

                                        EquityPartner cc = new EquityPartner();
                                        cc.EquityPartnerId = Guid.NewGuid(); ;
                                        cc.PartnerName = row[10].ToString().Trim();

                                        cc.CreatedDate = DateTime.Now;
                                        cc.CreatedBy = contact.CreatedBy;
                                        cc.CreatedByName = "Excel Upload";
                                        _entities.EquityPartners.Add(cc);
                                        _entities.SaveChanges();
                                        contact.EquityPartner = cc.EquityPartnerId;

                                    }

                                    change = true;
                                }
                            }
                            else
                                contact.EquityPartner = null;
                            // Asset Manager

                            if (!string.IsNullOrEmpty(row[11].ToString()))
                            {

                                if (othercontact.AssetManager.ToLower().Trim() != row[11].ToString().ToLower().Trim())
                                {
                                    // if changed, check contact exist with given name
                                    var searchstr = row[11].ToString().Trim().ToLower();
                                    var checkexists55 = _entities.proc_checkcontactexists(searchstr).FirstOrDefault();
                                    if (checkexists55 != null)
                                    {

                                        var chkcontact = (from tbl in _entities.Contacts
                                                          where tbl.ContactId == checkexists55.ContactId
                                                          select tbl).FirstOrDefault();

                                        if (chkcontact != null)
                                        {
                                            contact.AssetManager1 = chkcontact.ContactId;
                                            change = true;
                                        }
                                    }
                                    else
                                    {
                                        // create new contact with this Name and phone number and update contact id as vp for property table

                                        Contact c = new Contact();
                                        c.ContactId = Guid.NewGuid(); ;
                                        var sp = row[11].ToString().Split(new char[0]);

                                        c.FirstName = row[11].ToString().Split(new char[0])[0].ToString();
                                        if (sp.Length > 1)
                                        {
                                            c.LastName = row[11].ToString().Replace(c.FirstName, "");

                                            //if (row[7].ToString().Split(new char[0])[1] != null)
                                            //{
                                            //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                            //}

                                        }
                                        c.Title = "Asset Manager ";
                                        c.CreatedBy = contact.CreatedBy;
                                        c.CreatedDate = DateTime.Now;
                                        c.CreatedByName = contact.CreatedByName;
                                        _entities.Contacts.Add(c);
                                        _entities.SaveChanges();
                                        contact.AssetManager1 = c.ContactId;
                                        change = true;

                                    }
                                }

                            }
                            else
                            {
                                contact.AssetManager1 = null;
                            }

                            // Construction Manager -- 12 thc column
                            if (!string.IsNullOrEmpty(row[12].ToString()))
                            {
                                if (othercontact.ConstructionManager.ToLower().Trim() != row[12].ToString().ToLower().Trim())
                            {
                                // if changed, check contact exist with given name
                                var searchstr = row[12].ToString().Trim().ToLower();
                                    var checkexists55 = _entities.proc_checkcontactexists(searchstr).FirstOrDefault();
                                    if (checkexists55 != null)
                                    {

                                        var chkcontact = (from tbl in _entities.Contacts
                                                          where tbl.ContactId == checkexists55.ContactId
                                                          select tbl).FirstOrDefault();

                                        if (chkcontact != null)
                                        {
                                            contact.ConstructionManager = chkcontact.ContactId;
                                            change = true;
                                        }
                                    }
                                    else
                                    {
                                        // create new contact with this Name and phone number and update contact id as vp for property table

                                        Contact c = new Contact();
                                        c.ContactId = Guid.NewGuid(); ;
                                        var sp = row[12].ToString().Split(new char[0]);

                                        c.FirstName = row[12].ToString().Split(new char[0])[0].ToString();
                                        if (sp.Length > 1)
                                        {
                                            c.LastName = row[12].ToString().Replace(c.FirstName, "");

                                            //if (row[7].ToString().Split(new char[0])[1] != null)
                                            //{
                                            //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                            //}

                                        }
                                        c.Title = "Construction Manager ";
                                        c.CreatedBy = contact.CreatedBy;
                                        c.CreatedDate = DateTime.Now;
                                        c.CreatedByName = contact.CreatedByName;
                                        _entities.Contacts.Add(c);
                                        _entities.SaveChanges();
                                        contact.ConstructionManager = c.ContactId;
                                        change = true;

                                    }
                            }
                            }
                            else
                            {
                                contact.ConstructionManager = null;
                            }

                            // Marketing Specialist= -- 13 thc column
                            if (!string.IsNullOrEmpty(row[13].ToString()))
                            {
                                if (othercontact.MarketingSpecialist.ToLower().Trim() != row[13].ToString().ToLower().Trim())
                            {
                                // if changed, check contact exist with given name
                                var searchstr = row[13].ToString().Trim().ToLower();
                                    var checkexists55 = _entities.proc_checkcontactexists(searchstr).FirstOrDefault();
                                    if (checkexists55 != null)
                                    {

                                        var chkcontact = (from tbl in _entities.Contacts
                                                          where  tbl.ContactId == checkexists55.ContactId
                                                          select tbl).FirstOrDefault();

                                        if (chkcontact != null)
                                        {
                                            contact.MarketingSpecialist = chkcontact.ContactId;
                                            change = true;
                                        }
                                    }
                                    else
                                    {
                                        // create new contact with this Name and phone number and update contact id as vp for property table

                                        Contact c = new Contact();
                                        c.ContactId = Guid.NewGuid(); ;
                                        var sp = row[13].ToString().Split(new char[0]);

                                        c.FirstName = row[13].ToString().Split(new char[0])[0].ToString();
                                        if (sp.Length > 1)
                                        {
                                            c.LastName = row[13].ToString().Replace(c.FirstName, "");

                                            //if (row[7].ToString().Split(new char[0])[1] != null)
                                            //{
                                            //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                            //}

                                        }
                                        c.Title = "Marketing Specialist ";
                                        c.CreatedBy = contact.CreatedBy;
                                        c.CreatedDate = DateTime.Now;
                                        c.CreatedByName = contact.CreatedByName;
                                        _entities.Contacts.Add(c);
                                        _entities.SaveChanges();
                                        contact.MarketingSpecialist = c.ContactId;
                                        change = true;

                                    }
                            }

                            }
                            else
                            {
                                contact.MarketingSpecialist = null;
                            }


                            // Property Manager = -- 14 thc column
                            if (!string.IsNullOrEmpty(row[14].ToString()))
                            {
                                if (othercontact.PropertyManager.ToLower().Trim() != row[14].ToString().ToLower().Trim())
                            {
                                // if changed, check contact exist with given name
                                var searchstr = row[14].ToString().Trim().ToLower();
                                    var checkexists55 = _entities.proc_checkcontactexists(searchstr).FirstOrDefault();
                                    if (checkexists55 != null)
                                    {

                                        var chkcontact = (from tbl in _entities.Contacts
                                                          where  tbl.ContactId == checkexists55.ContactId
                                                          select tbl).FirstOrDefault();

                                        if (chkcontact != null)
                                        {
                                            contact.PropertyManager = chkcontact.ContactId;
                                            change = true;
                                        }
                                    }
                                    else
                                    {
                                        // create new contact with this Name and phone number and update contact id as vp for property table

                                        Contact c = new Contact();
                                        c.ContactId = Guid.NewGuid(); ;
                                        var sp = row[14].ToString().Split(new char[0]);

                                        c.FirstName = row[14].ToString().Split(new char[0])[0].ToString();
                                        if (sp.Length > 1)
                                        {
                                            c.LastName = row[14].ToString().Replace(c.FirstName, "");

                                            //if (row[7].ToString().Split(new char[0])[1] != null)
                                            //{
                                            //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                            //}

                                        }
                                        c.Title = "Property Manager";
                                        c.CreatedBy = contact.CreatedBy;
                                        c.CreatedDate = DateTime.Now;
                                        c.CreatedByName = contact.CreatedByName;
                                        _entities.Contacts.Add(c);
                                        _entities.SaveChanges();
                                        contact.PropertyManager = c.ContactId;
                                        change = true;

                                    }
                            }
                            }
                            else
                            {
                                contact.PropertyManager = null;
                            }



                            if (contact.PhoneNumber.ToLower().Trim() != row[15].ToString().ToLower().Trim())
                            {
                                contact.PhoneNumber = row[15].ToString().Trim();
                                change = true;
                            }

                            if (contact.PropertyAddress.ToLower().Trim() != row[16].ToString().ToLower().Trim())
                            {
                                contact.PropertyAddress = row[16].ToString().Trim();
                                change = true;
                            }



                            if (contact.City.ToLower().Trim() != row[17].ToString().ToLower().Trim())
                            {
                                contact.City = row[17].ToString().Trim();
                                change = true;
                            }

                            if (contact.State.ToLower().Trim() != row[18].ToString().ToLower().Trim())
                            {
                                contact.State = row[18].ToString().Trim();
                                change = true;
                            }

                            if (contact.ZipCode.ToLower().Trim() != row[19].ToString().ToLower().Trim())
                            {
                                contact.ZipCode = row[19].ToString().Trim();
                                change = true;
                            }


                            if (contact.EmailAddress.ToLower().Trim() != row[20].ToString().ToLower().Trim())
                            {
                                contact.EmailAddress = row[20].ToString().Trim();
                                change = true;
                            }

                            if (contact.LegalName.ToLower().Trim() != row[21].ToString().ToLower().Trim())
                            {
                                contact.LegalName = row[21].ToString().Trim();
                                change = true;
                            }

                            //if (contact.Purchase_TookOver.ToLower().Trim() != row[22].ToString().ToLower().Trim())
                            //{
                            //    contact.Purchase_TookOver = row[22].ToString().Trim();
                            //    change = true;
                            //}


                            //if (contact.AcquisitionDate.ToString() != row[23].ToString().Trim())
                            //{
                            //    if (!string.IsNullOrEmpty(row[23].ToString().Trim()))
                            //    {
                            //        var str = row[23].ToString().Trim().Replace("/", "-").Split('-');
                            //        contact.AcquisitionDate = Convert.ToDateTime(str[1] + "-" + str[0] + "-" + str[2]);
                            //    }
                            //    contact.AcquisitionDate = Convert.ToDateTime(row[23].ToString().Trim().Replace("/", "-"));
                            //    change = true;
                            //}

                            //if (contact.RefinancedDate.ToString() != row[24].ToString().Trim())
                            //{
                            //    if (!string.IsNullOrEmpty(row[24].ToString().Trim()))
                            //    {
                            //        var str = row[24].ToString().Trim().Replace("/", "-").Split('-');
                            //        contact.RefinancedDate = Convert.ToDateTime(str[1] + "-" + str[0] + "-" + str[2]);
                            //    }
                            //    contact.RefinancedDate = Convert.ToDateTime(row[24].ToString().Trim().Replace("/", "-"));
                            //    change = true;
                            //}

                            if (contact.TaxId != row[25].ToString().Trim())
                            {
                                contact.TaxId = row[25].ToString().Trim();
                                change = true;
                            }

                            if (change == true)
                            {
                                int i = _entities.SaveChanges();
                            }
                        }

                    }

                    if (row[0].ToString() == "" || isnewrow == true)
                        {
                            // create new property update all other details

                            // for all other contacts, if exist udpate contact or equitypartner id, if not create new and udpate 

                            Property contact1 = new Property();
                            contact1.PropertyId = Guid.NewGuid();;


                        // vp

                        // if changed, check contact exist with given name

                        if (!string.IsNullOrEmpty(row[1].ToString()))
                        {
                            var searchstr = row[1].ToString().Trim().ToLower();

                            //  var chkcontact = _entities.Contacts.ToList().Where(a => searchstr.All(p=>(a.FirstName+a.LastName).ToLower().Contains(p))).FirstOrDefault();
                            var checkexists51 = _entities.proc_checkcontactexists(searchstr).FirstOrDefault();
                            if (checkexists51 != null)
                            {
                                var chkcontact = (from tbl in _entities.Contacts
                                                  where tbl.ContactId == checkexists51.ContactId
                                                  select tbl).FirstOrDefault();

                                if (chkcontact != null)
                                {
                                    contact1.VicePresident = chkcontact.ContactId;
                                    chkcontact.Phone = row[2].ToString();
                                    change = true;
                                }
                            }
                            else
                            {
                                // create new contact with this Name and phone number and update contact id as vp for property table

                                Contact c = new Contact();
                                c.ContactId = Guid.NewGuid(); ;
                                var sp = row[1].ToString().Split(new char[0]);

                                c.FirstName = row[1].ToString().Split(new char[0])[0].ToString();
                                if (sp.Length > 1)
                                {
                                    c.LastName = row[1].ToString().Replace(c.FirstName, "");

                                    //if (row[7].ToString().Split(new char[0])[1] != null)
                                    //{
                                    //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                    //}

                                }
                                c.Title = "Vice President";
                                //  c.CreatedBy = contact.CreatedBy;
                                c.CreatedDate = DateTime.Now;
                                //  c.CreatedByName = contact.CreatedByName;
                                _entities.Contacts.Add(c);
                                _entities.SaveChanges();
                                contact1.VicePresident = c.ContactId;
                                change = true;
                            }

                        }
                        else
                        {
                            contact1.VicePresident = null;
                        }

                        if (!string.IsNullOrEmpty(row[4].ToString()))
                        {

                            // if changed, check contact exist with given name
                            var searchstr1 = row[3].ToString().Trim().ToLower();
                            var checkexists1 = _entities.proc_checkcontactexists(searchstr1).FirstOrDefault();
                            if (checkexists1 != null)
                            {

                                var chkcontact1 = (from tbl in _entities.Contacts
                                                   where tbl.ContactId == checkexists1.ContactId
                                                   select tbl).FirstOrDefault();

                                if (chkcontact1 != null)
                                {
                                    contact1.RegionalVicePresident = chkcontact1.ContactId;
                                    chkcontact1.Phone = row[4].ToString();
                                    change = true;
                                }
                            }
                            else
                            {
                                // create new contact with this Name and phone number and update contact id as vp for property table

                                Contact c = new Contact();
                                c.ContactId = Guid.NewGuid(); ;
                                var sp = row[3].ToString().Split(new char[0]);

                                c.FirstName = row[3].ToString().Split(new char[0])[0].ToString();
                                if (sp.Length > 1)
                                {
                                    c.LastName = row[3].ToString().Replace(c.FirstName, "");

                                    //if (row[7].ToString().Split(new char[0])[1] != null)
                                    //{
                                    //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                    //}

                                }
                                c.Title = "Regional Vice President";
                                //   c.CreatedBy = contact.CreatedBy;
                                c.CreatedDate = DateTime.Now;
                                //   c.CreatedByName = contact.CreatedByName;
                                _entities.Contacts.Add(c);
                                _entities.SaveChanges();
                                contact1.RegionalVicePresident = c.ContactId;
                                change = true;

                            }

                        }
                        else
                        {
                            contact1.RegionalVicePresident = null;
                        }


                        if (!string.IsNullOrEmpty(row[5].ToString()))
                        {


                            // For Regional Manager
                            // if changed, check contact exist with given name
                            var searchstr2 = row[5].ToString().Trim().ToLower();
                            var checkexists2 = _entities.proc_checkcontactexists(searchstr2).FirstOrDefault();
                            if (checkexists2 != null)
                            {

                                var chkcontact2 = (from tbl in _entities.Contacts
                                                   where tbl.ContactId == checkexists2.ContactId
                                                   select tbl).FirstOrDefault();

                                if (chkcontact2 != null)
                                {
                                    contact1.RegionalManager = chkcontact2.ContactId;
                                    chkcontact2.Phone = row[6].ToString();
                                    change = true;
                                }
                            }
                            else
                            {
                                // create new contact with this Name and phone number and update contact id as vp for property table

                                Contact c = new Contact();
                                c.ContactId = Guid.NewGuid(); ;
                                var sp = row[5].ToString().Split(new char[0]);

                                c.FirstName = row[5].ToString().Split(new char[0])[0].ToString();
                                if (sp.Length > 1)
                                {
                                    c.LastName = row[5].ToString().Replace(c.FirstName, "");

                                    //if (row[7].ToString().Split(new char[0])[1] != null)
                                    //{
                                    //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                    //}

                                }
                                c.Title = "Regional Manager ";
                                //    c.CreatedBy = contact.CreatedBy;
                                c.CreatedDate = DateTime.Now;
                                //  c.CreatedByName = contact.CreatedByName;
                                _entities.Contacts.Add(c);
                                _entities.SaveChanges();
                                contact1.RegionalManager = c.ContactId;
                                change = true;

                            }


                        }
                        else
                        {
                            contact1.RegionalManager = null;
                        }


                        if (!string.IsNullOrEmpty(row[10].ToString()))
                        {
                            var searchstr3 = row[10].ToString().Trim().ToLower();
                            // check if exist, exist udpate partnerid, if not then create new equitypartner, update id
                            var otherpartner = (from tbl in _entities.EquityPartners
                                                    where tbl.PartnerName.ToLower().Contains(searchstr3)
                                                    select tbl).FirstOrDefault();

                                if (otherpartner != null)
                                {
                                    contact1.EquityPartner = otherpartner.EquityPartnerId;
                                }
                                else
                                {
                                    // create new partner and update

                                    EquityPartner cc = new EquityPartner();
                                    cc.EquityPartnerId = Guid.NewGuid();;
                                    cc.PartnerName = row[10].ToString().Trim();

                                    cc.CreatedDate = DateTime.Now;
//cc.CreatedBy = contact.CreatedBy;
                                  //  cc.CreatedByName = "Excel Upload";
                                    _entities.EquityPartners.Add(cc);
                                    _entities.SaveChanges();
                                    contact1.EquityPartner = cc.EquityPartnerId;

                                }

                                change = true;
                        }
                        else
                        {
                            contact1.EquityPartner = null;
                        }

                        if (!string.IsNullOrEmpty(row[11].ToString()))
                        {
                            // Asset Manager
                            // if changed, check contact exist with given name

                            var searchstr4 = row[11].ToString().Trim().ToLower();
                            var checkexists4 = _entities.proc_checkcontactexists(searchstr4).FirstOrDefault();
                            if (checkexists4 != null)
                            {

                                var chkcontact3 = (from tbl in _entities.Contacts
                                                   where tbl.ContactId == checkexists4.ContactId
                                                   select tbl).FirstOrDefault();

                                if (chkcontact3 != null)
                                {
                                    contact1.AssetManager1 = chkcontact3.ContactId;
                                    change = true;
                                }
                            }
                            else
                            {
                                // create new contact with this Name and phone number and update contact id as vp for property table

                                Contact c = new Contact();
                                c.ContactId = Guid.NewGuid(); ;
                                var sp = row[11].ToString().Split(new char[0]);

                                c.FirstName = row[11].ToString().Split(new char[0])[0].ToString();
                                if (sp.Length > 1)
                                {
                                    c.LastName = row[11].ToString().Replace(c.FirstName, "");

                                    //if (row[7].ToString().Split(new char[0])[1] != null)
                                    //{
                                    //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                    //}

                                }
                                c.Title = "Asset Manager ";
                                //  c.CreatedBy = contact.CreatedBy;
                                c.CreatedDate = DateTime.Now;
                                //  c.CreatedByName = contact.CreatedByName;
                                _entities.Contacts.Add(c);
                                _entities.SaveChanges();
                                contact1.AssetManager1 = c.ContactId;
                                change = true;

                            }
                        }
                        else
                        {
                            contact1.AssetManager1 = null;
                        }

                        // Construction Manager -- 12 thc column

                        if (!string.IsNullOrEmpty(row[12].ToString()))
                        {
                            var searchstr5 = row[12].ToString().Trim().ToLower();
                            var checkexists5 = _entities.proc_checkcontactexists(searchstr5).FirstOrDefault();
                            if (checkexists5 != null)
                            {

                                var chkcontact4 = (from tbl in _entities.Contacts
                                                   where tbl.ContactId == checkexists5.ContactId
                                                   select tbl).FirstOrDefault();

                                if (chkcontact4 != null)
                                {
                                    contact1.ConstructionManager = chkcontact4.ContactId;
                                    change = true;
                                }
                            }
                            else
                            {
                                // create new contact with this Name and phone number and update contact id as vp for property table

                                Contact c = new Contact();
                                c.ContactId = Guid.NewGuid(); ;
                                var sp = row[12].ToString().Split(new char[0]);

                                c.FirstName = row[12].ToString().Split(new char[0])[0].ToString();
                                if (sp.Length > 1)
                                {
                                    c.LastName = row[12].ToString().Replace(c.FirstName, "");

                                    //if (row[7].ToString().Split(new char[0])[1] != null)
                                    //{
                                    //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                    //}

                                }
                                c.Title = "Construction Manager ";
                                c.CreatedBy = new Guid("F0C3A30B-50A8-4E20-A0B5-5B6AA0BC9B4E");
                                c.CreatedDate = DateTime.Now;
                                //  c.CreatedByName = contact.CreatedByName;
                                _entities.Contacts.Add(c);
                                _entities.SaveChanges();
                                contact1.ConstructionManager = c.ContactId;
                                change = true;

                            }

                        }
                        else
                        {
                            contact1.ConstructionManager = null;
                        }

                        if (!string.IsNullOrEmpty(row[13].ToString()))
                        {
                            // Marketing Specialist= -- 13 thc column
                            var searchstr6 = row[13].ToString().Trim().ToLower();
                            var checkexists6 = _entities.proc_checkcontactexists(searchstr6).FirstOrDefault();
                            if (checkexists6 != null)
                            {

                                var chkcontact5 = (from tbl in _entities.Contacts
                                                   where tbl.ContactId == checkexists6.ContactId
                                                   select tbl).FirstOrDefault();

                                if (chkcontact5 != null)
                                {
                                    contact1.MarketingSpecialist = chkcontact5.ContactId;
                                    change = true;
                                }
                            }
                            else
                            {
                                // create new contact with this Name and phone number and update contact id as vp for property table

                                Contact c = new Contact();
                                c.ContactId = Guid.NewGuid(); ;
                                var sp = row[13].ToString().Split(new char[0]);

                                c.FirstName = row[13].ToString().Split(new char[0])[0].ToString();
                                if (sp.Length > 1)
                                {
                                    c.LastName = row[13].ToString().Replace(c.FirstName, "");

                                    //if (row[7].ToString().Split(new char[0])[1] != null)
                                    //{
                                    //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                    //}

                                }
                                c.Title = "Marketing Specialist ";
                                ///  c.CreatedBy = contact.CreatedBy;
                                c.CreatedDate = DateTime.Now;
                                //  c.CreatedByName = contact.CreatedByName;
                                _entities.Contacts.Add(c);
                                _entities.SaveChanges();
                                contact1.MarketingSpecialist = c.ContactId;
                                change = true;

                            }

                        }
                        else
                        {
                            contact1.MarketingSpecialist = null;
                        }



                        // Property Manager = -- 14 thc column
                        if (!string.IsNullOrEmpty(row[14].ToString()))
                        {


                            var searchstr7 = row[14].ToString().Trim().ToLower();
                            var checkexists = _entities.proc_checkcontactexists(searchstr7).FirstOrDefault();
                            if(checkexists!= null)
                            {

                            
                            var chkcontact6 = (from tbl in _entities.Contacts
                                                  where tbl.ContactId == checkexists.ContactId
                                               select tbl).FirstOrDefault();

                                if (chkcontact6 != null)
                                {
                                    contact1.PropertyManager = chkcontact6.ContactId;
                                    change = true;
                                }
                            }
                            else
                                {
                                    // create new contact with this Name and phone number and update contact id as vp for property table

                                    Contact c = new Contact();
                                    c.ContactId = Guid.NewGuid();;
                                    var sp = row[14].ToString().Split(new char[0]);

                                    c.FirstName = row[14].ToString().Split(new char[0])[0].ToString();
                                    if (sp.Length > 1)
                                    {
                                        c.LastName = row[14].ToString().Replace(c.FirstName, "");

                                        //if (row[7].ToString().Split(new char[0])[1] != null)
                                        //{
                                        //    c.LastName = row[7].ToString().Split(new char[0])[1].ToString();
                                        //}

                                    }
                                    c.Title = "Property Manager";
                                 //   c.CreatedBy = contact.CreatedBy;
                                    c.CreatedDate = DateTime.Now;
                                 //   c.CreatedByName = contact.CreatedByName;
                                    _entities.Contacts.Add(c);
                                    _entities.SaveChanges();
                                    contact1.PropertyManager = c.ContactId;
                                    change = true;

                                }


                        }
                        else
                        {
                            contact1.PropertyManager = null;
                        }

                        if (!string.IsNullOrEmpty(row[7].ToString()))
                                contact1.PropertyNumber = Convert.ToInt16(row[7].ToString().Trim());

                            if (!string.IsNullOrEmpty(row[8].ToString()))
                                contact1.Units = Convert.ToInt16(row[8].ToString().Trim());

                            if (!string.IsNullOrEmpty(row[8].ToString()))
                                contact1.Units = Convert.ToInt16(row[8].ToString().Trim());

                            contact1.PropertyName = row[9].ToString().Trim();

                            contact1.PhoneNumber = row[15].ToString().Trim();

                            contact1.PropertyAddress = row[16].ToString().Trim();
                            contact1.City = row[17].ToString().Trim();
                            contact1.State = row[18].ToString().Trim();
                            contact1.ZipCode = row[19].ToString().Trim();
                            contact1.EmailAddress = row[20].ToString().Trim();
                            contact1.LegalName = row[21].ToString().Trim();
                        contact1.Purchase_TookOver = row[22].ToString().Trim();
                        if (!string.IsNullOrEmpty(row[23].ToString().Trim()))
                        {
                            var str = row[23].ToString().Trim().Replace("/", "-").Split('-');
                            contact1.AcquisitionDate = Convert.ToDateTime(str[1] + "-" + str[0] + "-" + str[2]);
                        }
                        //    contact1.AcquisitionDate = Convert.ToDateTime(row[23].ToString().Trim().Replace("/","-").Substring();

                        if (!string.IsNullOrEmpty(row[24].ToString().Trim()))
                        {
                            var str = row[24].ToString().Trim().Replace("/", "-").Split('-');
                            contact1.RefinancedDate = Convert.ToDateTime(str[1] + "-" + str[0] + "-" + str[2]);
                        }
                         //   contact1.RefinancedDate = Convert.ToDateTime(row[24].ToString().Trim().Replace("/", "-"));
                        contact1.TaxId = row[25].ToString().Trim();
                       
                     
                            contact1.CreatedDate = DateTime.Now;
                            contact1.CreatedByName = "Excel Upload";
                            _entities.Properties.Add(contact1);
                            _entities.SaveChanges();
                        }
                   
                }


            }
            return true;
                      
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
        public dynamic UpdateRequisitionRequest(Guid Refid, string RequisitionNumber,string Notes, DateTime dateposted)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                var res = (from tbl in _entities.RequisitionRequests
                           where tbl.RequisitionRequestId == Refid
                           select tbl).FirstOrDefault();
                res.RequistionNumber = RequisitionNumber;
                res.DatePosted = dateposted;
                res.Notes = Notes;
                res.ModifiedDateTime = DateTime.Now;
                _entities.SaveChanges();

                return true;

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

                if(propertyres != null)
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


        public string GetPropertyNumberNameManager(string PropertyNumber)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                var propid = new Guid(PropertyNumber);
                //us
                var propertyres = (from tbl in _entities.Properties
                                   where tbl.PropertyId == propid
                                   select new { tbl.PropertyName, tbl.PropertyManager,tbl.PropertyNumber }).FirstOrDefault();
                var manager = "";
                if (propertyres.PropertyManager != null)
                {

                    var mananger1 = (from tbl in _entities.Contacts
                                     where tbl.ContactId == propertyres.PropertyManager
                                     select new { tbl.FirstName, tbl.LastName, tbl.Email }).FirstOrDefault();
                    manager = mananger1.FirstName + " " + mananger1.LastName;

                }

                if (propertyres != null)
                    return propertyres.PropertyName + "," + manager+","+propertyres.PropertyNumber;
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

     public dynamic GetHrFormLogActivity(string FormType, string RecordId)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                
                var rid = new Guid(RecordId);
                var res = _entities.proc_getallhrformsactivity(FormType, rid).ToList();
                var dldetails = _entities.proc_getactivitylog(FormType, rid).ToList();
                //var dldetails = (from tbl in _entities.DynamicLinks
                //                 where tbl.FormType == FormType && tbl.ReferenceId == rid && tbl.IpAddress != null && tbl.BrowserInformation!= null
                //                 orderby tbl.CreatedDate descending
                //                 select new { browserinfo = tbl.BrowserInformation, ip = tbl.IpAddress, datetime = tbl.Clientdatetime, tbl.Action }).ToList();

                if (FormType =="NewHire")
                {
                  //  var res1 = _entities.proc_getnewhirerejectiondetails(rid).ToList();

                    var res1 = _entities.proc_getnewhirerejectionhistory(rid).ToList();

                    //var res1 = (from tbl in _entities.EmployeeNewHireNotices
                    //           join tbluser in _entities.SiteUsers on tbl.RejectedBy equals tbluser.UserId
                    //           where tbl.EmployeeHireNoticeId == rid
                    //           select new { tbl.RejectedReason, tbluser.FirstName, tbluser.LastName, RejectedDateTime= tbl.RejectedDateTime.Value.ToString("MM/dd/yyyy") + " " + tbl.RejectedDateTime.Value.ToShortTimeString() }).ToList();
                    return new { log = res, metadata = dldetails,rejection=res1 };
                }
               else
                    return new { log = res, metadata = dldetails,rejection= "" };

            }
        }

       public dynamic GetDynamicLinkStatus(Guid refid)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                return (from tbl in _entities.DynamicLinks
                        where tbl.DynamicLinkId == refid
                        select tbl.OpenStatus).FirstOrDefault();
            }
        }
        public dynamic UpdateNewHireRejectionStatus(string status, string reason, string refid, string refuser)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {

                var id = new Guid(refid);

                var propertyres = (from tbl in _entities.EmployeeNewHireNotices
                                   where tbl.EmployeeHireNoticeId == id
                                   select tbl).FirstOrDefault();

                NewHireRejectionHistory rh = new NewHireRejectionHistory();


                if(propertyres != null)
                {
                    if(status == "reject")
                    {
                        propertyres.IsRejected = true;
                        propertyres.RejectedBy = new Guid(refuser);
                        propertyres.RejectedDateTime = DateTime.Now;
                        propertyres.RejectedReason = reason;
                        propertyres.PmSignedDateTime = null;
                        propertyres.EmployeeSignedDateTime = null;
                        propertyres.RegionalManagerSignedDateTime = null;
                        rh.ClientDateTime = DateTime.Now;
                        rh.HistoryID = Guid.NewGuid();
                        rh.NewHireId = new Guid(refid);
                        rh.RejectedUser= new Guid(refuser);
                        rh.RejectionDesc = reason;
                        _entities.NewHireRejectionHistories.Add(rh);
                        _entities.SaveChanges();
                    }
                    else if(status =="cancel")
                    {
                        propertyres.IsRejected = true;
                        propertyres.RejectedBy = new Guid(refuser);
                        propertyres.RejectedDateTime = DateTime.Now;
                        propertyres.RejectedReason = "cancel";
                        propertyres.PmSignedDateTime = null;
                        propertyres.EmployeeSignedDateTime = null;
                        propertyres.RegionalManagerSignedDateTime = null;
                        rh.ClientDateTime = DateTime.Now;
                        rh.HistoryID = Guid.NewGuid();
                        rh.NewHireId = new Guid(refid);
                        rh.RejectedUser = new Guid(refuser);
                        rh.RejectionDesc = "cancel";
                        _entities.NewHireRejectionHistories.Add(rh);
                        _entities.SaveChanges();
                    }
                    _entities.proc_closealllinks(id);
                }

                return true;

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

    public class ResidentReferralResidents
    {
        public System.Guid ResidentId { get; set; }
        public Nullable<System.Guid> ResidentContactInformationId { get; set; }
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public Nullable<bool> Home_Work { get; set; }
        public string Home_Work_Phone { get; set; }
        public string CurrentEmployer { get; set; }
        public string Position { get; set; }

    }


    public class ResidentReferralOthers
    {
        public System.Guid OccupantId { get; set; }
        public Nullable<System.Guid> ResidentContactInformationId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
    }

    public class ResidentReferralVehicles
    {
        public System.Guid VehicleId { get; set; }
        public Nullable<System.Guid> ResidentContactInformationId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }
        public string LicensePlatState { get; set; }
    }


    public class PrintResidentContact
    {
        public System.Guid Contactid { get; set; }
        public string Building { get; set; }
        public string Apartment { get; set; }
        public string PropertyName { get; set; }
        public string ReturnEmail { get; set; }
        public string Fax1 { get; set; }
        public string Fax11 { get; set; }
        public string Fax2 { get; set; }
        public string Fax22 { get; set; }
        public string InsuranceDeclaration { get; set; }
        public string Em_name { get; set; }
        public string Em_Address { get; set; }
        public string Em_Phone { get; set; }
        public string Em_Relation { get; set; }
        public string ResidentSingature1 { get; set; }
        public Nullable<System.DateTime> ResidentSignDate1 { get; set; }
        public string ResidentSingature2 { get; set; }
        public Nullable<System.DateTime> ResidentSignDate2 { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public List<ResidentReferralResidents> rrs { get; set; }
        public List<ResidentReferralOthers> ros { get; set; }
        public List<ResidentReferralVehicles> rvs { get; set; }
        
    }






    //public class PrintResidentContact
    //{
    //    public ResidentContactInformation contact { get; set; }
    //    public List<ResidentContactInformation_Residents> rrs { get; set; }
    //    public List<ResidentContactInformation_OtherOccupants> ros { get; set; }
    //    public List<ResidentContactInformation_Vehicles> rvs { get; set; }
    //}

}
