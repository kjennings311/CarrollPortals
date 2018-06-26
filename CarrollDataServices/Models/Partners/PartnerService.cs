using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using Carroll.Data.Services.Models.Validation;


namespace Carroll.Data.Services.Models.Partners
{
    public class PartnerService :IPartnerService
    {
        private IValidationDictionary _validationDictionary;
        private IPartnerRepository _repository;
       // private EntityUserRepository entityUserRepository;

        public PartnerService(IValidationDictionary validationDictionary) 
            : this(validationDictionary, new EntityPartnerRepository())
        {}

        public PartnerService(IValidationDictionary validationDictionary, IPartnerRepository repository)
        {
            _validationDictionary = validationDictionary;
            _repository = repository;
        }


      
        #region [Contacts]
        public List<Carroll.Data.Entities.EquityPartner> GetAllPartners(string optionalSeachText = "")
        {
            return _repository.GetEquityPartners(optionalSeachText);
        }
        #endregion
    }
}
