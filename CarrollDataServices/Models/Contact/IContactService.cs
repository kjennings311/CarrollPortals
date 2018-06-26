using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
namespace Carroll.Data.Services.Models.Contact
{
    public interface IContactService
    {
      
        List<Carroll.Data.Entities.Contact> GetAllContacts(string optionalSeachText = "");

        Carroll.Data.Entities.Contact GetContact(string ContactId);

        bool CreateUpdateContact(Carroll.Data.Entities.Contact Contact);
        bool DeleteContact(string ContactId);
    }
}
