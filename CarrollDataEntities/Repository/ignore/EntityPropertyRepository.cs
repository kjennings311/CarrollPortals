using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities.Helpers;
using System.Data.Entity;

namespace Carroll.Data.Entities.Repository
{
    public class EntityPropertyRepository:IPropertyRepository
    {
        public CarrollFormsEntities DBEntity => new EFConnectionAccessor().Entities;
     

      
        /// <summary>
        /// Returns All properties from Database
        /// </summary>
        /// <param name="optionalSeachText">Looks for occurance in PropertyName, LegalName</param>
        /// <returns></returns>
        public List<spProperties_Result> GetProperties(string optionalSeachText = "")
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                if (string.IsNullOrEmpty(optionalSeachText)) return _entities.spProperties().ToList();
                else return _entities.spProperties().Where(x => x.PropertyName.Contains(optionalSeachText) || x.LegalName.Contains(optionalSeachText) || x.PropertyId.ToString().Contains(optionalSeachText)).ToList();
                
            }
        }


        /// <summary>
        /// Create or update property
        /// </summary>
        /// <param name="Property"></param>
        /// <returns></returns>
        public bool CreateUpdateProperty(Property Property)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                var _property = _entities.Properties.Where(x => x.PropertyId == Property.PropertyId).FirstOrDefault();
                if (_property == null)
                {
                    if((Property.PropertyId.ToString() == "00000000-0000-0000-0000-000000000000") || (Property.PropertyId == null))
                    {
                        Property.PropertyId = Guid.NewGuid();
                    }
                    Property.CreatedDate = DateTime.Now;
                    // No record exists create a new property record here
                    //_property = new Property();
                    //_property.PropertyId
                   // Property.PropertyId = null;
                    _entities.Properties.Add(Property);
                    _entities.SaveChanges();
                    int i = _entities.SaveChanges();
                    return (i == 1) ? true : false;
                }
                else {

                    //_entities.Properties.Attach(Property);
                    //_entities.Entry(Property).State = EntityState.Modified;
                    _entities.Entry(_property).CurrentValues.SetValues(Property);
                    int i = _entities.SaveChanges();
                    // return (i == 1) ? true : false;
                    return true;

                }
            }

        }

        public bool DeleteProperty(string PropertyId)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                // Let's first delete related records first.. then we will delete the main record that will trigger clear in formdata table
                _entities.Database.ExecuteSqlCommand("DELETE FROM PROPERTIES WHERE PROPERTYID={0} ", PropertyId);

                return true;

            }
           
        }

        public Property GetProperty(string PropertyId)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                Guid _propId = new Guid(PropertyId);
                var _property = _entities.Properties.Where(x => x.PropertyId == _propId).FirstOrDefault();
                if (_property != null) { return _property; }
                return null;
            }

        }


        public dynamic GetUserPropertyForClaimPrint(string PropertyId)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                PrintProperty pp = new PrintProperty();

                _entities.Configuration.ProxyCreationEnabled = false;
                Guid _propId = new Guid(PropertyId);

                var propertyres = _entities.proc_getpropertydetails(_propId).FirstOrDefault();

                if (propertyres != null)
                    return propertyres;
                else
                    return null;
            }
              
            

        }
    }
    public class PrintProperty
    {
        public string PropertyName { get; set; }
        public string LegalName { get; set; }
        public string Units { get; set; }
        public string YardiCode { get; set; }
        public string Taxid { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string EquityPartner { get; set; }
        public string VicePresident { get; set; }
        public string RegionalVicePresident { get; set; }
        public string PropertyManager { get; set; }
        public string AssetManager { get; set; }
        public string RegionalManager { get; set; }
        public string ConstructionManager { get; set; }
        //public string Address { get; set; }
        //public string Address { get; set; }

    }
}
