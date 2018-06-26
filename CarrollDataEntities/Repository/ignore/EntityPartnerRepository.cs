using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities.Helpers;

namespace Carroll.Data.Entities.Repository
{
    public class EntityPartnerRepository:IPartnerRepository
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

  

     

    }
}
