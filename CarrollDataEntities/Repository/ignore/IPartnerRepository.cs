
    using System;
    using System.Collections.Generic;
    using System.Linq;

namespace Carroll.Data.Entities.Repository
{
    /// <summary>
    /// Summary description for IAdminRepository
    /// </summary>
    public interface IPartnerRepository
    {
        List<EquityPartner> GetEquityPartners(string optionalSeachText = "");

       
    }
}