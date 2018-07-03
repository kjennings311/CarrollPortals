using System;
using System.Collections.Generic;
using System.Linq;
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
                            _entities.SaveChanges();
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
                    default:
                        // if we are here that means it must be a dynamic object.. Let's try to evaluate and insert..

                        Type _objType = obj?.GetType();

                        break;
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
                    case EntityType.User:
                      
                        return null;
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

            var type = Type.GetType("Carroll.Data.Entities." + className);
            return Activator.CreateInstance(type);

        }
    }
}
