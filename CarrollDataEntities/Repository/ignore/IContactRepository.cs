
    using System;
    using System.Collections.Generic;
    using System.Linq;

namespace Carroll.Data.Entities.Repository
{
    /// <summary>
    /// Summary description for IAdminRepository
    /// </summary>
    public interface IContactRepository
    {
        List<EquityPartner> GetEquityPartners(string optionalSeachText = "");

        List<Contact> GetContacts(string optionalSeachText = "");

        List<Contact> GetAllContacts(string optionalSeachText = "");

        Carroll.Data.Entities.Contact GetContact(string ContactId);

        bool DeleteContact(string ContactId);

        bool CreateUpdateContact(Carroll.Data.Entities.Contact Contact);
    }
}