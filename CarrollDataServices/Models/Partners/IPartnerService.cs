using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
namespace Carroll.Data.Services.Models.Partners
{
    public interface IPartnerService
    {
      
        List<Carroll.Data.Entities.EquityPartner> GetAllPartners(string optionalSeachText = "");
    }
}
