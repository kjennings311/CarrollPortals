using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities.Helpers;

namespace Carroll.Data.Entities.Repository
{
    public class EntityContactRepository:IContactRepository
    {
        public CarrollFormsEntities DBEntity => new EFConnectionAccessor().Entities;

        public List<EquityPartner> GetEquityPartners(string optionalSeachText = "")
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;

                if (string.IsNullOrEmpty(optionalSeachText)) return _entities.EquityPartners.ToList();
                else return _entities.EquityPartners.Where(x => x.IsActive && (x.PartnerName.Contains(optionalSeachText) || x.AddressLine1.Contains(optionalSeachText) || x.AddressLine2.Contains(optionalSeachText) || x.City.Contains(optionalSeachText) || x.State.Contains(optionalSeachText))).ToList();

            }
        }

  

        public List<Contact> GetContacts(string optionalSeachText = "")
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                if (string.IsNullOrEmpty(optionalSeachText)) return _entities.Contacts.ToList();
                else return _entities.Contacts.Where(x => x.IsActive && (x.FirstName.Contains(optionalSeachText) || x.LastName.Contains(optionalSeachText) || x.Title.Contains(optionalSeachText) || x.Phone.Contains(optionalSeachText) || x.Email.Contains(optionalSeachText))).ToList();


            }

        }

        #region [Contacts ] 
        public List<Contact> GetAllContacts(string optionalSeachText = "")
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                if (string.IsNullOrEmpty(optionalSeachText)) return _entities.Contacts.ToList();
                else return _entities.Contacts.Where(x => x.FirstName.Contains(optionalSeachText) || x.LastName.Contains(optionalSeachText) || x.Email.Contains(optionalSeachText) || x.Phone.Contains(optionalSeachText) || x.Title.Contains(optionalSeachText)).ToList();

            }
        }

        public Contact GetContact(string ContactId)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                Guid _contactId = new Guid(ContactId);
                return _entities.Contacts.Where(x => x.ContactId == _contactId).FirstOrDefault();

            }
        }

        public bool DeleteContact(string ContactId)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                // Let's first delete related records first.. then we will delete the main record that will trigger clear in formdata table
                _entities.Database.ExecuteSqlCommand("DELETE FROM CONTACTS WHERE CONTACTID={0} ", ContactId);

                return true;

            }
        }

        public bool CreateUpdateContact(Carroll.Data.Entities.Contact Contact)
        {
            using (CarrollFormsEntities _entities = DBEntity)
            {
                _entities.Configuration.ProxyCreationEnabled = false;
                var _contact = _entities.Contacts.Where(x => x.ContactId == Contact.ContactId).FirstOrDefault();
                if (_contact == null)
                {
                    if ((Contact.ContactId.ToString() == "00000000-0000-0000-0000-000000000000") || (Contact.ContactId == null))
                    {
                        Contact.ContactId = Guid.NewGuid();
                    }
                    Contact.CreatedDate = DateTime.Now;
                    // No record exists create a new property record here
                    //_property = new Property();
                    //_property.PropertyId
                    // Property.PropertyId = null;
                    _entities.Contacts.Add(Contact);
                    _entities.SaveChanges();
                    int i = _entities.SaveChanges();
                    // return (i == 1) ? true : false;
                    return true;
                }
                else
                {

                    //_entities.Properties.Attach(Property);
                    //_entities.Entry(Property).State = EntityState.Modified;
                    _entities.Entry(_contact).CurrentValues.SetValues(Contact);
                    int i = _entities.SaveChanges();
                    // return (i == 1) ? true : false;
                    return true;

                }
            }

        }
        #endregion

    }
}
